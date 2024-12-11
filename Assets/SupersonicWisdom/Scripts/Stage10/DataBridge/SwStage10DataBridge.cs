#if SW_STAGE_STAGE10_OR_ABOVE
using System.Collections.Generic;
using UnityEngine;

namespace SupersonicWisdomSDK
{
    internal class SwStage10DataBridge : SwCoreDataBridge
    {
        #region --- Constants ---

        private const string DATE_TIME_FORMAT = "yyyy-MM-dd HH:mm";

        #endregion


        #region --- Members ---

        protected readonly ISwNativeApi _nativeApi;
        protected readonly SwSettings _settings;
        protected readonly SwStage10UserData _userData;
        protected readonly SwTimerManager _timerManager;
        protected ISwRevenueCalculator _revenueCalculator;

        #endregion


        #region --- Construction ---

        public SwStage10DataBridge(SwStage10UserData userData, ISwNativeApi nativeApi, SwSettings settings, SwTimerManager timerManager)
        {
            _nativeApi = nativeApi;
            _userData = userData;
            _settings = settings;
            _timerManager = timerManager;
        }

        #endregion
        
        
        #region --- Public Methods ---

        internal void SetRevenueCalculator(ISwRevenueCalculator revenueCalculator)
        {
            _revenueCalculator ??= revenueCalculator;
        }

        #endregion


        #region --- Private Methods ---

        protected override void AddDataToDictionary(Dictionary<string, object> data, ESwGetDataFlag flag)
        {
            switch (flag)
            {
                case ESwGetDataFlag.ActiveDay:
                    data.SwAddOrReplace(SwStage10DataBridgeKeys.ACTIVE_DAY_KEY, GetActiveDay());
                    break;

                case ESwGetDataFlag.TotalNetoPlayTime:
                    data.SwAddOrReplace(SwStage10DataBridgeKeys.TOTAL_NETO_PLAY_TIME, GetTotalNetoPlayTime());
                    break;
                
                case ESwGetDataFlag.MainLevel:
                    data.SwAddOrReplace(SwStage10DataBridgeKeys.MAIN_LEVEL, GetMainLevel());
                    break;
                
                case ESwGetDataFlag.TotalRevenue:
                    data.SwAddOrReplace(SwStage10DataBridgeKeys.TOTAL_REVENUE, GetTotalRevenue());
                    break;
                
                case ESwGetDataFlag.InstallDate:
                    data.SwAddOrReplace(SwStage10DataBridgeKeys.INSTALL_DATE, GetInstallDateTime());
                    break;
                
                case ESwGetDataFlag.SessionId:
                    data.SwAddOrReplace(SwStage10DataBridgeKeys.SESSION_ID, GetSessionId());
                    break;
                
                case ESwGetDataFlag.MegaSessionId:
                    data.SwAddOrReplace(SwStage10DataBridgeKeys.MEGA_SESSION_ID, GetMegaSessionId());
                    break;
                
                case ESwGetDataFlag.BundleId:
                    data.SwAddOrReplace(SwStage10DataBridgeKeys.BUNDLE_ID, GetBundleId());
                    break;
                
                case ESwGetDataFlag.SupersonicId:
                    data.SwAddOrReplace(SwStage10DataBridgeKeys.SUPERSONIC_ID, GetSupersonicId());
                    break;
                
                case ESwGetDataFlag.AppVersion:
                    data.SwAddOrReplace(SwStage10DataBridgeKeys.APP_VERSION, GetAppVersion());
                    break;
                
                case ESwGetDataFlag.Os:
                    data.SwAddOrReplace(SwStage10DataBridgeKeys.OS, GetOs());
                    break;
                
                case ESwGetDataFlag.OsVersion:
                    data.SwAddOrReplace(SwStage10DataBridgeKeys.OS_VERSION, GetOsVersion());
                    break;
#if UNITY_IOS
                case ESwGetDataFlag.IosStoreId:
                    data.SwAddOrReplace(SwStage10DataBridgeKeys.IOS_STORE_ID, GetIosStoreId());
                    break;
#endif
            }
        }
        
        private int GetTotalNetoPlayTime()
        {
            return (int)Mathf.Round(_timerManager.AllSessionsPlaytimeNeto);
        }

        private long GetMainLevel()
        {
            return _userData.ImmutableUserState().lastLevelStarted;
        }
        
        private double GetTotalRevenue()
        {
            return _revenueCalculator.Revenue;    
        }
        
        private string GetInstallDateTime()
        {
            return _userData.InstallDateTime.SwToString(DATE_TIME_FORMAT);
        }

        private string GetSessionId()
        {
            return _userData.ImmutableUserState().SessionId;
        }

        private string GetMegaSessionId()
        {
            return _nativeApi.GetMegaSessionId();
        }

        private string GetBundleId()
        {
            return SwUtils.System.BundleIdentifier;
        }

        private string GetIosStoreId()
        {
#if UNITY_IOS
            return _settings.iosChinaBuildEnabled ? _settings.iosChinaAppId : _settings.iosAppId;
#else
            return string.Empty;
#endif
        }

        private string GetSupersonicId()
        {
#if UNITY_IOS
            return _settings.iosChinaBuildEnabled ? _settings.iosChinaGameId : _settings.iosGameId;
#else
            return _settings.androidGameId;
#endif
        }

        private string GetAppVersion()
        {
            return SwUtils.System.AppVersion;
        }

        private string GetOs()
        {
            return SwUtils.System.PlatformDisplayName;
        }

        private string GetOsVersion()
        {
            return SwUtils.System.PlatformVersion;
        }

        private string GetCountryCode()
        {
            if (DataDictionary.ContainsKey(SwStage10DataBridgeKeys.COUNTRY_CODE) && !DataDictionary[SwStage10DataBridgeKeys.COUNTRY_CODE].ToString().SwIsNullOrEmpty())
            {
                return DataDictionary[SwStage10DataBridgeKeys.COUNTRY_CODE].ToString();
            }

            return SwUtils.LangAndCountry.GetCountry();
        }
        
        private int GetActiveDay()
        {
            return _userData.ActiveDay;
        }

        #endregion
    }
}
#endif