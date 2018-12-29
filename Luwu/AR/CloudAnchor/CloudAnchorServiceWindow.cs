using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Net;
using System.Net.Sockets;
using System.Net.NetworkInformation;

namespace Luwu.CloudAnchors
{
    using Luwu.UI;

    public class CloudAnchorServiceWindow : UIWidget
    {
        /// <summary>
        /// A gameobject parenting UI for displaying feedback and errors.
        /// </summary>
        public Text SnackbarText;

        /// <summary>
        /// A text element displaying the current Room.
        /// </summary>
        public Text RoomText;

        /// <summary>
        /// A text element displaying the device's IP Address.
        /// </summary>
        public Text IPAddressText;

        /// <summary>
        /// The host anchor mode button.
        /// </summary>
        public Button HostAnchorModeButton;

        /// <summary>
        /// The resolve anchor mode button.
        /// </summary>
        public Button ResolveAnchorModeButton;

        /// <summary>
        /// The root for the input interface.
        /// </summary>
        public GameObject InputRoot;

        /// <summary>
        /// The input field for the room.
        /// </summary>
        public InputField RoomInputField;

        /// <summary>
        /// The input field for the ip address.
        /// </summary>
        public InputField IpAddressInputField;

        /// <summary>
        /// The field for toggling loopback (local) anchor resoltion.
        /// </summary>
        public Toggle ResolveOnDeviceToggle;

        private Button mResolveRoomBtn;

        private CloudAnchorService mService;

        private System.Action<Transform> mOnCreateAnchorHandler;
        private System.Action<Transform> mOnResolveAnchorHandler;

        public override void Init(Transform trans)
        {
            base.Init(trans);

            SnackbarText = trans.Find("Snackbar").GetComponentInChildren<Text>();
            RoomText = trans.Find("Room Label").GetComponentInChildren<Text>();
            IPAddressText = trans.Find("IP Address Label").GetComponentInChildren<Text>();

            HostAnchorModeButton = trans.Find("Host Anchor Mode Button").GetComponent<Button>();
            ResolveAnchorModeButton = trans.Find("Resolve Anchor Mode Button").GetComponent<Button>();
            InputRoot = trans.Find("Room Input Root").gameObject;
            RoomInputField = InputRoot.transform.Find("Room Input").GetComponent<InputField>();
            IpAddressInputField = InputRoot.transform.Find("IP Address Input").GetComponent<InputField>();
            ResolveOnDeviceToggle = InputRoot.GetComponentInChildren<Toggle>();
            mResolveRoomBtn = InputRoot.transform.Find("Resolve Button").GetComponent<Button>();

            HostAnchorModeButton.onClick.AddListener(() =>
            {
                if(HostAnchorModeButton.GetComponentInChildren<Text>().text == "Cancel")
                {
                    mService.CancelHostingAnchor();
                    ShowReadyMode();
                }
                else
                {
                    ShowHostingModeBegin();
                    int room = Random.Range(1000, 9999);
                    IPAddressText.text = _GetDeviceIpAddress();
                    RoomText.text = room.ToString();
                    mService.HostingAnchor(room, (anchor) =>
                    {
                        if (mOnCreateAnchorHandler != null)
                        {
                            mOnCreateAnchorHandler(anchor);
                        }
                    });
                }

            });

            ResolveAnchorModeButton.onClick.AddListener(() =>
            {
                if(ResolveAnchorModeButton.GetComponentInChildren<Text>().text == "Cancel")
                {
                    mService.CancelResolveAnchor();
                    ShowReadyMode();
                }
                else
                {
                    ShowResolvingModeBegin();
                }
            });

            mResolveRoomBtn.onClick.AddListener(() =>
            {
                string ip = IpAddressInputField.text;
                string room = RoomInputField.text;
                bool resolveOnDevice = ResolveOnDeviceToggle.isOn;

                var roomToResolve = 0;
                if (!int.TryParse(room, out roomToResolve) || roomToResolve == 0)
                {
                    ShowResolvingModeBegin("Anchor resolve failed due to invalid room code.");
                    return;
                }

                SetRoomTextValue(roomToResolve);
                string ipAddress =
                    resolveOnDevice ? "127.0.0.1" : ip;

                mService.ResolveAnchor(ip, roomToResolve, 
                   (anchor) =>
                  {
                      mOnResolveAnchorHandler(anchor);
                      ShowResolvingModeSuccess();
                  }, 
                   (response) =>
                  {
                      ShowResolvingModeBegin(string.Format("Resolving Error: {0}.", response));
                  });

            });

            IPAddressText.text = "My IP Address: " + GetAddressIP();
            ShowReadyMode();
        }

        public void ShowReadyMode()
        {
            HostAnchorModeButton.GetComponentInChildren<Text>().text = "Host";
            HostAnchorModeButton.interactable = true;
            ResolveAnchorModeButton.GetComponentInChildren<Text>().text = "Resolve";
            ResolveAnchorModeButton.interactable = true;
            SnackbarText.text = "Please select Host or Resolve to continue";
            InputRoot.SetActive(false);
        }

        /// <summary>
        /// Shows UI for the beginning phase of application "Hosting Mode".
        /// </summary>
        /// <param name="snackbarText">Optional text to put in the snackbar.</param>
        public void ShowHostingModeBegin(string snackbarText = null)
        {
            HostAnchorModeButton.GetComponentInChildren<Text>().text = "Cancel";
            HostAnchorModeButton.interactable = true;
            ResolveAnchorModeButton.GetComponentInChildren<Text>().text = "Resolve";
            ResolveAnchorModeButton.interactable = false;

            if (string.IsNullOrEmpty(snackbarText))
            {
                SnackbarText.text =
                    "The room code is now available. Please place an anchor to host, press Cancel to Exit.";
            }
            else
            {
                SnackbarText.text = snackbarText;
            }

            InputRoot.SetActive(false);
        }

        /// <summary>
        /// Shows UI for the attempting to host phase of application "Hosting Mode".
        /// </summary>
        public void ShowHostingModeAttemptingHost()
        {
            HostAnchorModeButton.GetComponentInChildren<Text>().text = "Cancel";
            HostAnchorModeButton.interactable = false;
            ResolveAnchorModeButton.GetComponentInChildren<Text>().text = "Resolve";
            ResolveAnchorModeButton.interactable = false;
            SnackbarText.text = "Attempting to host anchor...";
            InputRoot.SetActive(false);
        }

        /// <summary>
        /// Shows UI for the beginning phase of application "Resolving Mode".
        /// </summary>
        /// <param name="snackbarText">Optional text to put in the snackbar.</param>
        public void ShowResolvingModeBegin(string snackbarText = null)
        {
            HostAnchorModeButton.GetComponentInChildren<Text>().text = "Host";
            HostAnchorModeButton.interactable = false;
            ResolveAnchorModeButton.GetComponentInChildren<Text>().text = "Cancel";
            ResolveAnchorModeButton.interactable = true;

            if (string.IsNullOrEmpty(snackbarText))
            {
                SnackbarText.text = "Input Room and IP address to resolve anchor.";
            }
            else
            {
                SnackbarText.text = snackbarText;
            }

            InputRoot.SetActive(true);
        }

        /// <summary>
        /// Shows UI for the attempting to resolve phase of application "Resolving Mode".
        /// </summary>
        public void ShowResolvingModeAttemptingResolve()
        {
            HostAnchorModeButton.GetComponentInChildren<Text>().text = "Host";
            HostAnchorModeButton.interactable = false;
            ResolveAnchorModeButton.GetComponentInChildren<Text>().text = "Cancel";
            ResolveAnchorModeButton.interactable = false;
            SnackbarText.text = "Attempting to resolve anchor.";
            InputRoot.SetActive(false);
        }

        /// <summary>
        /// Shows UI for the successful resolve phase of application "Resolving Mode".
        /// </summary>
        public void ShowResolvingModeSuccess()
        {
            HostAnchorModeButton.GetComponentInChildren<Text>().text = "Host";
            HostAnchorModeButton.interactable = false;
            ResolveAnchorModeButton.GetComponentInChildren<Text>().text = "Cancel";
            ResolveAnchorModeButton.interactable = true;
            SnackbarText.text = "The anchor was successfully resolved.";
            InputRoot.SetActive(false);
        }

        /// <summary>
        /// Sets the room number in the UI.
        /// </summary>
        /// <param name="roomNumber">The room number to set.</param>
        public void SetRoomTextValue(int roomNumber)
        {
            RoomText.text = "Room: " + roomNumber;
        }

        /// <summary>
        /// Gets the value of the resolve on device checkbox.
        /// </summary>
        /// <returns>The value of the resolve on device checkbox.</returns>
        public bool GetResolveOnDeviceValue()
        {
            return ResolveOnDeviceToggle.isOn;
        }

        /// <summary>
        /// Gets the value of the room number input field.
        /// </summary>
        /// <returns>The value of the room number input field.</returns>
        public int GetRoomInputValue()
        {
            int roomNumber;
            if (int.TryParse(RoomInputField.text, out roomNumber))
            {
                return roomNumber;
            }

            return 0;
        }

        /// <summary>
        /// Gets the value of the ip address input field.
        /// </summary>
        /// <returns>The value of the ip address input field.</returns>
        public string GetIpAddressInputValue()
        {
            return IpAddressInputField.text;
        }

        /// <summary>
        /// Handles a change to the "Resolve on Device" checkbox.
        /// </summary>
        /// <param name="isResolveOnDevice">If set to <c>true</c> resolve on device.</param>
        public void OnResolveOnDeviceValueChanged(bool isResolveOnDevice)
        {
            IpAddressInputField.interactable = !isResolveOnDevice;
        }

        /// <summary>
        /// Gets the device ip address.
        /// </summary>
        /// <returns>The device ip address.</returns>
        private string _GetDeviceIpAddress()
        {
            string ipAddress = "Unknown";
#if UNITY_2018_2_OR_NEWER
            string hostName = Dns.GetHostName();
            IPAddress[] addresses = Dns.GetHostAddresses(hostName);

            foreach (IPAddress address in addresses)
            {
                if (address.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                {
                    ipAddress = address.ToString();
                    break;
                }
            }
#else
            ipAddress = Network.player.ipAddress;
#endif
            return ipAddress;
        }

        public string GetAddressIP()
        {
            string addressip = string.Empty;
#if UNITY_IPHONE
            NetworkInterface[] adapters = NetworkInterface.GetAllNetworkInterfaces();
            for (int i = 0; i < adapters.Length; ++i)
            {
                if(adapters[i].Id != "en0")
                {
                    continue;
                }
                if (adapters[i].Supports(NetworkInterfaceComponent.IPv4))
                {
                    UnicastIPAddressInformationCollection unicast = adapters[i].GetIPProperties().UnicastAddresses;
                    if (unicast.Count > 0)
                    {
                        for (int j = 0; j < unicast.Count; ++j)
                        {
                            if (unicast[j].Address.AddressFamily == AddressFamily.InterNetwork)
                            {
                                addressip = unicast[j].Address.ToString();
                                Debug.Log("addressip:" + addressip);
                            }
                        }
                    }
                }
            }
#endif
#if UNITY_STANDALONE_WIN || UNITY_ANDROID
        for(int i=0;i<Dns.GetHostEntry(Dns.GetHostName()).AddressList.Length;++i)
        {
            if(Dns.GetHostEntry(Dns.GetHostName()).AddressList[i].AddressFamily.ToString() == "InterNetwork")
            {
                addressip = Dns.GetHostEntry(Dns.GetHostName()).AddressList[i].ToString();
            }
        }
#endif
            return addressip;
        }

        public void SetAnchorCreatedHandler(System.Action<Transform> handler)
        {
            mOnCreateAnchorHandler = handler;
        }

        public void SetAnchorResolvedHandler(System.Action<Transform> handler){
            mOnResolveAnchorHandler = handler;
        }

        public void SetCloudService(CloudAnchorService service){
            mService = service;
        }
    }
}

