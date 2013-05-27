using System;
using System.Collections.Generic;
using System.Threading;

namespace CertificateScanner.ExceptionDecor
{
    /// 
    /// Handles a thread (unhandled) exception.
    /// 
    public class ThreadExceptionHandler
    {
        /// 
        /// Handles the thread exception.
        /// 
        public static void Application_ThreadException(
            object sender, ThreadExceptionEventArgs e)
        {
            if (!(e.Exception is ExceptionWrapper))
                ExceptionDecorator.Error(e.Exception);
            switch ((e.Exception as ExceptionWrapper).type)
	        {
                case ExceptionWrapper.ErrorTypes.Error : 
                {
                    if ((e.Exception as ExceptionWrapper).withMessage)
                        ExceptionDecorator.Error((e.Exception as ExceptionWrapper).ex, (e.Exception as ExceptionWrapper).message);
                    else
                        ExceptionDecorator.Error((e.Exception as ExceptionWrapper).ex);
                    break;
                };
                    
                case ExceptionWrapper.ErrorTypes.Fatal : 
                {
                    if ((e.Exception as ExceptionWrapper).withMessage)
                        ExceptionDecorator.Fatal((e.Exception as ExceptionWrapper).ex, (e.Exception as ExceptionWrapper).message);
                    else
                        ExceptionDecorator.Fatal((e.Exception as ExceptionWrapper).ex);
                    break;
                };

                case ExceptionWrapper.ErrorTypes.Warning : 
                {
                    if ((e.Exception as ExceptionWrapper).withMessage)
                        ExceptionDecorator.Warn((e.Exception as ExceptionWrapper).ex, (e.Exception as ExceptionWrapper).message);
                    else
                        ExceptionDecorator.Warn((e.Exception as ExceptionWrapper).ex);
                    break;
                };

                case ExceptionWrapper.ErrorTypes.Info : 
                {
                    if ((e.Exception as ExceptionWrapper).withMessage)
                        ExceptionDecorator.Info((e.Exception as ExceptionWrapper).comment, (e.Exception as ExceptionWrapper).message, (e.Exception as ExceptionWrapper).data);
                    else
                        ExceptionDecorator.Info((e.Exception as ExceptionWrapper).comment, (e.Exception as ExceptionWrapper).data);
                    break;
                };

                case ExceptionWrapper.ErrorTypes.Trace : 
                {
                    ExceptionDecorator.Trace(); 
                    break;
                };

                case ExceptionWrapper.ErrorTypes.Debug : 
                {
                    ExceptionDecorator.Trace();
                    break;
                };

		        default:
                {
                    if ((e.Exception as ExceptionWrapper).withMessage)
                        ExceptionDecorator.Error((e.Exception as ExceptionWrapper).ex, (e.Exception as ExceptionWrapper).message);
                    else
                        ExceptionDecorator.Error((e.Exception as ExceptionWrapper).ex);
                    break;
                };
	        }
        }

        
    } // End ThreadExceptionHandler
}
