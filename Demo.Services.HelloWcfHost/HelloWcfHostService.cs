using System.Timers;
using System.ServiceModel;

using Demo.Services.Core;

namespace Demo.Services.HelloWcfHost
{
    public partial class HelloWcfHostService : CustomServiceBase
    {
        private Timer m_Timer;
        private bool m_Stopping = false;
        private ServiceHost m_HelloWcfServiceHost;

        public HelloWcfHostService()
        {
            InitializeComponent();
            InitializeTimer();
        }

        private void InitializeTimer()
        {
            m_Timer = new Timer()
            {
                AutoReset = true,
                Interval = 5000
            };
            m_Timer.Elapsed += new ElapsedEventHandler(m_Timer_Elapsed);
        }

        private void m_Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            if (m_Stopping)
            {
                m_Timer.Enabled = false;
                m_Timer.Dispose();
            }
        }

        protected override void OnStart(string[] args)
        {
            m_Timer.Enabled = true;

            InitializeHelloWcfServiceHost();
        }

        private void InitializeHelloWcfServiceHost()
        {
            m_HelloWcfServiceHost = new CustomServiceHost<HelloWcf.HelloWcf>();
            m_HelloWcfServiceHost.Open();
        }

        protected override void OnStop()
        {
            m_Stopping = true;
            m_HelloWcfServiceHost.Close();
        }
    }
}