using UnityEngine;
using UnityEngine.Purchasing;
using System.Collections;

public class IAPManager : IStoreListener {

    private const string PUBLIC_KEY = "MIIBIjANBgkqhkiG9w0BAQEFAAOCAQ8AMIIBCgKCAQEAgn6ZtETp2+B8Z3Xfj9RtKc2ITgbEvtmBrXn++fsLnzWi2+ZxTPsix+BawZ0xYZ0dkKOFrHZP/ZAcanKChqx9kSmSPThdTzSpw39Q+r1IDq1g1HvFbsP528NxhgPas14p13r66Lw3W1nPr5waQt9m8Fd5wM464a9PnyIDmAspgxnTbVtoploYFP5KJ7miXa17qyzQUgml9NXQCaQseMcGcKaTDROItnnBYA7AlfKHibbSJtqWq47iPGPTGRpfLlYj7ne5Nq53XWXTS0sfKzaSRcG0OOc0oI+bI+LVTkGHylJ57v6x0Py01xE+Dcfc/tP7HC6R+9JCzahH6XOeeBzgjwIDAQAB";

    private static IAPManager _instance;
    private static IAPManager Instance {
        get { 
            if (_instance == null) _instance = new IAPManager();
            return _instance;
        }
    }

    // Unity IAP objects 
    private IStoreController m_Controller;
    private IExtensionProvider m_Extensions;

    private bool m_Initialized = false;
    private bool m_PurchaseInProgress = false;
    public bool Initialized {
        get { return m_Initialized; }
    }

    #region UnityCallbacks
    /// <summary>
    /// This will be called when Unity IAP has finished initialising.
    /// </summary>
    public void OnInitialized(IStoreController controller, IExtensionProvider extensions) {
        m_Controller = controller;
        m_Extensions = extensions; ;

        m_Initialized = true;
        
        //InitUI(controller.products.all);

        // On Apple platforms we need to handle deferred purchases caused by Apple's Ask to Buy feature.
        // On non-Apple platforms this will have no effect; OnDeferred will never be called.
        //m_AppleExtensions.RegisterPurchaseDeferredListener(OnDeferred);

        Debug.Log("Available items:");
        foreach (var item in controller.products.all) {
            if (item.availableToPurchase) {
                Debug.Log(string.Join(" - ",
                    new[]
                    {
                        item.metadata.localizedTitle,
                        item.metadata.localizedDescription,
                        item.metadata.isoCurrencyCode,
                        item.metadata.localizedPrice.ToString(),
                        item.metadata.localizedPriceString
                    }));
            }
        }

        //// Prepare model for purchasing
        //if (m_Controller.products.all.Length > 0) {
        //    m_SelectedItemIndex = 0;
        //}

        //// Populate the product menu now that we have Products
        //for (int t = 0; t < m_Controller.products.all.Length; t++) {
        //    var item = m_Controller.products.all[t];
        //    var description = string.Format("{0} - {1}", item.metadata.localizedTitle, item.metadata.localizedPriceString);

        //    // NOTE: my options list is created in InitUI
        //    GetDropdown().options[t] = new Dropdown.OptionData(description);
        //}

        //// Ensure I render the selected list element
        //GetDropdown().RefreshShownValue();

        //// Now that I have real products, begin showing product purchase history
        //UpdateHistoryUI();
    }

    /// <summary>
    /// This will be called when a purchase completes.
    /// </summary>
    public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs e) {
        Debug.Log("Purchase OK: " + e.purchasedProduct.definition.id);
        Debug.Log("Receipt: " + e.purchasedProduct.receipt);

        m_PurchaseInProgress = false;

        // Now that my purchase history has changed, update its UI
        //UpdateHistoryUI();

#if RECEIPT_VALIDATION
		if (Application.platform == RuntimePlatform.Android ||
			Application.platform == RuntimePlatform.IPhonePlayer ||
			Application.platform == RuntimePlatform.OSXPlayer) {
			try {
				var result = validator.Validate(e.purchasedProduct.receipt);
				Debug.Log("Receipt is valid. Contents:");
				foreach (IPurchaseReceipt productReceipt in result) {
					Debug.Log(productReceipt.productID);
					Debug.Log(productReceipt.purchaseDate);
					Debug.Log(productReceipt.transactionID);

					GooglePlayReceipt google = productReceipt as GooglePlayReceipt;
					if (null != google) {
						Debug.Log(google.purchaseState);
						Debug.Log(google.purchaseToken);
					}

					AppleInAppPurchaseReceipt apple = productReceipt as AppleInAppPurchaseReceipt;
					if (null != apple) {
						Debug.Log(apple.originalTransactionIdentifier);
						Debug.Log(apple.cancellationDate);
						Debug.Log(apple.quantity);
					}
				}
			} catch (IAPSecurityException) {
				Debug.Log("Invalid receipt, not unlocking content");
				return PurchaseProcessingResult.Complete;
			}
		}
#endif

        // You should unlock the content here.

        // Indicate we have handled this purchase, we will not be informed of it again.x
        return PurchaseProcessingResult.Complete;
    }

    /// <summary>
    /// This will be called is an attempted purchase fails.
    /// </summary>
    public void OnPurchaseFailed(Product item, PurchaseFailureReason r) {
        Debug.Log("Purchase failed: " + item.definition.id);
        Debug.Log(r);

        m_PurchaseInProgress = false;
    }

    public void OnInitializeFailed(InitializationFailureReason error) {
        Debug.Log("Billing failed to initialize!");
        switch (error) {
            case InitializationFailureReason.AppNotKnown:
                Debug.LogError("Is your App correctly uploaded on the relevant publisher console?");
                break;
            case InitializationFailureReason.PurchasingUnavailable:
                // Ask the user if billing is disabled in device settings.
                Debug.Log("Billing disabled!");
                break;
            case InitializationFailureReason.NoProductsAvailable:
                // Developer configuration error; check product metadata.
                Debug.Log("No products available for purchase!");
                break;
        }
    }
    #endregion Unirt

    public static void Start() {
        if (!Instance.Initialized) Instance.Initialize();
    }

    public static void Purchase(string id) {
        Instance.PurchaseItem(id);
    }

    public void Initialize() {
        StandardPurchasingModule module = StandardPurchasingModule.Instance();

        // The FakeStore supports: no-ui (always succeeding), basic ui (purchase pass/fail), and 
        // developer ui (initialization, purchase, failure code setting). These correspond to 
        // the FakeStoreUIMode Enum values passed into StandardPurchasingModule.useFakeStoreUIMode.
        //module.useFakeStoreUIMode = FakeStoreUIMode.DeveloperUser;
        
        // Define products.
        ConfigurationBuilder builder = ConfigurationBuilder.Instance(module);
        builder.AddProduct("testprod", ProductType.Consumable, new IDs
		{
			{"testprod", MacAppStore.Name},
			{"testprod", GooglePlay.Name},
		});
  
#if RECEIPT_VALIDATION
		validator = new CrossPlatformValidator(GooglePlayTangle.Data(), AppleTangle.Data(), Application.bundleIdentifier);
#endif

        // Now we're ready to initialize Unity IAP.
        UnityPurchasing.Initialize(this, builder);
    }


    public void PurchaseItem(string id) {
        if (m_PurchaseInProgress == true) {
            return;
        }

        m_Controller.InitiatePurchase(m_Controller.products.WithID(id));

        // Don't need to draw our UI whilst a purchase is in progress.
        // This is not a requirement for IAP Applications but makes the demo
        // scene tidier whilst the fake purchase dialog is showing.
        m_PurchaseInProgress = true;
    }
}
