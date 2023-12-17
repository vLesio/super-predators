using System.Collections.Generic;
using CoinPackage.Debugging;

namespace Utils {
    public static class Loggers {
        public static Dictionary<LoggerType, CLogger> LoggersList;

        public enum LoggerType {
            UTILS,
            APPLICATION,
            PARAM_SELECTOR
        }
        
        static Loggers() {
            LoggersList = new Dictionary<LoggerType, CLogger> {
                {
                    LoggerType.UTILS,
                    new CLogger(LoggerType.UTILS) {
                        LogEnabled = true
                    }
                },
                {
                    LoggerType.APPLICATION,
                    new CLogger(LoggerType.APPLICATION) {
                        LogEnabled = true
                    }
                },
                {
                    LoggerType.PARAM_SELECTOR,
                    new CLogger(LoggerType.PARAM_SELECTOR) {
                        LogEnabled = true
                    }
                }
            };
        }
    }
}