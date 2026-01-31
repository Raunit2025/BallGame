using UnityEngine;
using UnityEngine.Purchasing;
using UnityEngine.Purchasing.Security; // Required for CrossPlatformValidator
using System;
using System.Collections.Generic;
using System.Linq;
using Unity.Services.Core;
using System.Threading.Tasks;
using Newtonsoft.Json; // Make sure you have JSON.NET or use JsonUtility

public class IAPManager : MonoBehaviour
{
    public static IAPManager Instance { get; private set; }
    
    // Event sends the purchased quantity
    public static event Action<int> OnPurchaseSuccessful;

    private StoreController m_StoreController;
    private bool m_IsStoreInitialized = false;

    public const string PRODUCT_BUY_100_BALLS = "buy_100_balls";

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        InitializeIAP();
    }

    async void InitializeIAP()
    {
        Debug.Log("IAPManager: Starting initialization...");
        try
        {
            await UnityServices.InitializeAsync();
            Debug.Log("IAPManager: UGS Handshake SUCCESS.");
        }
        catch (Exception e)
        {
            Debug.LogError($"IAPManager: UGS Initialization failed: {e.Message}");
            return;
        }

        m_StoreController = UnityIAPServices.StoreController();
        m_StoreController.OnPurchasePending += OnPurchasePending;
        m_StoreController.OnProductsFetched += OnProductsFetched;
        m_StoreController.OnProductsFetchFailed += OnProductsFetchedFailed;
        m_StoreController.OnPurchaseDeferred += OnPurchaseDeferred;

        Debug.Log("IAPManager: Connecting to Google Play...");
        try
        {
            var connectTask = m_StoreController.Connect();
            if (await Task.WhenAny(connectTask, Task.Delay(10000)) == connectTask)
            {
                await connectTask;
                m_IsStoreInitialized = true;
                Debug.Log("IAPManager: SUCCESSFULLY CONNECTED TO STORE.");
                FetchProducts();
            }
            else
            {
                Debug.LogError("IAPManager: Connection TIMED OUT. Check License Key.");
            }
        }
        catch (Exception e)
        {
            Debug.LogError($"IAPManager: CONNECTION FAILED: {e.Message}");
        }
    }

    void FetchProducts()
    {
        var productsToFetch = new List<ProductDefinition>
        {
            new(PRODUCT_BUY_100_BALLS, ProductType.Consumable)
        };
        Debug.Log("IAPManager: Fetching products...");
        m_StoreController.FetchProducts(productsToFetch);
    }

    public void Buy100Balls()
    {
        if (!m_IsStoreInitialized || m_StoreController == null)
        {
            Debug.LogError("IAPManager: Cannot buy. Store is NOT initialized.");
            return;
        }

        var product = m_StoreController.GetProductById(PRODUCT_BUY_100_BALLS);
        if (product != null && product.availableToPurchase)
        {
            Debug.Log($"IAPManager: Initiating purchase for {PRODUCT_BUY_100_BALLS}");
            // Note: Unity IAP's InitiatePurchase typically buys 1 unit. 
            // Multi-quantity selection usually happens in the Store UI or requires a custom payload if supported.
            m_StoreController.PurchaseProduct(PRODUCT_BUY_100_BALLS);
        }
        else
        {
            Debug.LogError($"IAPManager: Product {PRODUCT_BUY_100_BALLS} not found.");
        }
    }

    // --- Event Callbacks ---

    void OnPurchaseDeferred(DeferredOrder deferredOrder)
    {
        Debug.Log("IAPManager: Purchase deferred.");
    }

    void OnProductsFetched(List<Product> products)
    {
        Debug.Log($"IAPManager: {products.Count} products found in Google Play.");
    }

    void OnPurchasePending(PendingOrder order)
    {
        foreach (var item in order.CartOrdered.Items()) 
        {
            var product = item.Product;
            Debug.Log($"IAPManager: Purchase pending for: {product?.definition.id}");

            if (product?.definition.id == PRODUCT_BUY_100_BALLS)
            {
                // 1. Try standard quantity
                int quantity = item.Quantity;

                // 2. If standard is 1, try to parse receipt for deeper data (Android specific)
                if (quantity <= 1)
                {
                    // This is a simplified check. Real parsing requires the UnifiedReceipt class
                    // For now, we will trust the item.Quantity BUT log it clearly.
                    // If you are buying "2x" in the Google Sandbox, it might send 2 separate orders of 1.
                }

                Debug.Log($"IAPManager: PROCESSING PURCHASE. Quantity: {quantity}");
                
                // IMPORTANT: If you receive multiple separate events for 1 item each, this will trigger twice.
                OnPurchaseSuccessful?.Invoke(quantity);
            }
        }
        m_StoreController.ConfirmPurchase(order);
    }

    void OnProductsFetchedFailed(ProductFetchFailed failure)
    {
        Debug.LogError($"IAPManager: Products fetch failed. Reason: {failure.FailureReason}");
    }

    private void OnDestroy()
    {
        if (m_StoreController != null)
        {
            m_StoreController.OnPurchasePending -= OnPurchasePending;
            m_StoreController.OnProductsFetched -= OnProductsFetched;
            m_StoreController.OnProductsFetchFailed -= OnProductsFetchedFailed;
            m_StoreController.OnPurchaseDeferred -= OnPurchaseDeferred;
        }
    }
}