using UnityEngine;
using UnityEngine.Purchasing;
using System;
using System.Collections.Generic;
using System.Linq; // Needed for .First()

// Note: We no longer use IAPCore or IStoreListener
public class IAPManager : MonoBehaviour
{
    public static IAPManager Instance { get; private set; }
    public static event Action OnPurchaseSuccessful;

    private StoreController m_StoreController;

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
        // Start the async initialization
        InitializeIAP();
    }

    async void InitializeIAP()
    {
        // Get the controller instance
        m_StoreController = UnityIAPServices.StoreController();

        // Subscribe to all the new events
        m_StoreController.OnPurchasePending += OnPurchasePending;
        m_StoreController.OnPurchaseConfirmed += OnPurchaseConfirmed;
        m_StoreController.OnPurchaseFailed += OnPurchaseFailed;
        m_StoreController.OnStoreDisconnected += OnStoreDisconnected;
        m_StoreController.OnProductsFetchFailed += OnProductsFetchedFailed;
        m_StoreController.OnProductsFetched += OnProductsFetched;

        Debug.Log("IAPManager: Connecting to store...");
        try
        {
            // Asynchronously connect to the store
            await m_StoreController.Connect();
        }
        catch (Exception e)
        {
            Debug.LogError($"IAPManager: Failed to connect to store: {e.Message}");
            return;
        }

        // After connecting, fetch our products
        FetchProducts();
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

    // --- Public Purchase Method ---

    public void Buy100Balls()
    {
        if (m_StoreController == null)
        {
            Debug.LogWarning("IAPManager: Cannot buy, Store Controller is not initialized.");
            return;
        }
        m_StoreController.PurchaseProduct(PRODUCT_BUY_100_BALLS);
    }

    // --- Event Callbacks ---

    void OnProductsFetched(List<Product> products)
    {
        Debug.Log($"IAPManager: Products fetched successfully ({products.Count} products).");
        foreach (var product in products)
        {
            Debug.Log($"Product: {product.definition.id}, Available: {product.availableToPurchase}");
        }
    }

    void OnPurchasePending(PendingOrder order)
    {
        var product = GetFirstProductInOrder(order);
        if (product is null)
        {
            Debug.LogWarning("IAPManager: Could not find product in pending order.");
            return;
        }

        Debug.Log($"IAPManager: Purchase pending for Product: {product.definition.id}");

        // Grant the item
        if (product.definition.id == PRODUCT_BUY_100_BALLS)
        {
            OnPurchaseSuccessful?.Invoke();
        }

        // CRITICAL: Confirm the purchase
        m_StoreController.ConfirmPurchase(order);
    }

    void OnPurchaseConfirmed(Order order)
    {
        // This callback confirms that the store (e.g., Google Play)
        // has successfully registered the confirmation.
        
        var product = GetFirstProductInOrder(order);

        // Check if the confirmation itself failed
        if (order is FailedOrder failedOrder)
        {
            Debug.LogError($"IAPManager: Confirmation FAILED for {product?.definition.id}. Reason: {failedOrder.FailureReason}, Details: {failedOrder.Details}");
            return;
        }
        
        Debug.Log($"IAPManager: Purchase confirmed successfully for Product: {product?.definition.id}");
    }

    void OnPurchaseFailed(FailedOrder order)
    {
        var product = GetFirstProductInOrder(order);
        Debug.LogError($"IAPManager: Purchase FAILED for {product?.definition.id}. Reason: {order.FailureReason}, Details: {order.Details}");
    }
    
    // --- Helper & Cleanup Methods ---

    Product GetFirstProductInOrder(Order order)
    {
        return order.CartOrdered.Items().First()?.Product;
    }

    void OnStoreDisconnected(StoreConnectionFailureDescription description)
    {
        Debug.LogWarning($"IAPManager: Store disconnected. Message: {description.message}");
    }

    void OnProductsFetchedFailed(ProductFetchFailed failure)
    {
        Debug.LogError($"IAPManager: Products fetch failed. Reason: {failure.FailureReason}");
    }

    private void OnDestroy()
    {
        // Unsubscribe from events
        if (m_StoreController != null)
        {
            m_StoreController.OnPurchasePending -= OnPurchasePending;
            m_StoreController.OnPurchaseConfirmed -= OnPurchaseConfirmed;
            m_StoreController.OnPurchaseFailed -= OnPurchaseFailed;
            m_StoreController.OnStoreDisconnected -= OnStoreDisconnected;
            m_StoreController.OnProductsFetchFailed -= OnProductsFetchedFailed;
            m_StoreController.OnProductsFetched -= OnProductsFetched;
        }
    }
}