using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using Autofac;
using BrockAllen.MembershipReboot;
using BrockAllen.MembershipReboot.Ef;
using BrockAllen.MembershipReboot.Owin;
using BrockAllen.MembershipReboot.Relational;
using Microsoft.Owin;
using Owin;

namespace Flip.Areas.UserAccount
{
    public class UserAccountModule : Module
    {
        public UserAccountModule(string authenticationType, IAppBuilder owinAppBuilder)
        {
            this.AuthenticationType = authenticationType;
            this.OwinAppBuilder = owinAppBuilder;
        }

        /// <summary>
        /// The middleware authentication type. You might set it to "Cookie" or the value of `MembershipRebootOwinConstants.AuthenticationType`.
        /// </summary>
        public string AuthenticationType { get; private set; }
        public IAppBuilder OwinAppBuilder { get; private set; }

        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterInstance(CreateMembershipRebootConfiguration(this.OwinAppBuilder))
                .AsSelf();

            builder.RegisterType<DefaultMembershipRebootDatabase>()
                .InstancePerLifetimeScope();

            builder.RegisterType<DefaultUserAccountRepository>()
                .As<IUserAccountRepository>()
                .As<IUserAccountRepository<RelationalUserAccount>>()
                .As<IUserAccountQuery>()
                .As<IUserAccountQuery<BrockAllen.MembershipReboot.Relational.RelationalUserAccount>>()
                .InstancePerLifetimeScope();

            builder.RegisterType<UserAccountService>().OnActivating(e =>
            {
                var owin = e.Context.Resolve<IOwinContext>();
                var debugging = false;
#if DEBUG
                debugging = true;
#endif
                e.Instance.ConfigureTwoFactorAuthenticationCookies(owin.Environment, debugging);
            })
            .AsSelf()
            .InstancePerLifetimeScope();

            builder.RegisterType<UserAccountService<RelationalUserAccount>>().OnActivating(e =>
            {
                var owin = e.Context.Resolve<IOwinContext>();
                var debugging = false;
#if DEBUG
                debugging = true;
#endif
                e.Instance.ConfigureTwoFactorAuthenticationCookies(owin.Environment, debugging);
            })
            .AsSelf()
            .InstancePerLifetimeScope();

            builder.Register(ctx =>
            {
                var owin = ctx.Resolve<IOwinContext>();
                return new OwinAuthenticationService(AuthenticationType, ctx.Resolve<UserAccountService>(), owin.Environment);
            })
            .As<AuthenticationService>()
            .InstancePerLifetimeScope();
        }

        private static MembershipRebootConfiguration CreateMembershipRebootConfiguration(IAppBuilder app)
        {
            var config = new MembershipRebootConfiguration();
            config.EmailIsUsername = true;
            config.RequireAccountVerification = true;
            config.AddEventHandler(new DebuggerEventHandler());

            var appInfo = new OwinApplicationInformation(
                app: app,
                appName: "Test",
                emailSig: "Test Email Signature",
                relativeLoginUrl: "/useraccount/login",
                relativeConfirmChangeEmailUrl: "/useraccount/changeemail/confirm/",
                relativeCancelNewAccountUrl: "/useraccount/register/cancel/",
                relativeConfirmPasswordResetUrl: "/useraccount/passwordreset/confirm/");

            var emailFormatter = new EmailMessageFormatter(appInfo);
            // uncomment if you want email notifications -- also update smtp settings in web.config
            config.AddEventHandler(new EmailAccountEventsHandler(emailFormatter));
            // uncomment to enable SMS notifications -- also update TwilloSmsEventHandler class below
            //config.AddEventHandler(new TwilloSmsEventHandler(appinfo));

            // uncomment to ensure proper password complexity
            //config.ConfigurePasswordComplexity();

            return config;
        }
    }

    public class TwilloSmsEventHandler : SmsEventHandler
    {
        const string sid = "";
        const string token = "";
        const string fromPhone = "";

        public TwilloSmsEventHandler(ApplicationInformation appInfo)
            : base(new SmsMessageFormatter(appInfo))
        {
        }

        string Url
        {
            get
            {
                return String.Format("https://api.twilio.com/2010-04-01/Accounts/{0}/SMS/Messages", sid);
            }
        }

        string BasicAuthToken
        {
            get
            {
                var val = sid + ":" + token;
                var bytes = System.Text.Encoding.UTF8.GetBytes(val);
                val = Convert.ToBase64String(bytes);
                return val;
            }
        }

        HttpContent GetBody(Message msg)
        {
            var values = new KeyValuePair<string, string>[]
            { 
                new KeyValuePair<string, string>("From", fromPhone),
                new KeyValuePair<string, string>("To", msg.To),
                new KeyValuePair<string, string>("Body", msg.Body),
            };

            return new FormUrlEncodedContent(values);
        }

        protected override void SendSms(Message message)
        {
            if (!String.IsNullOrWhiteSpace(sid))
            {
                var client = new HttpClient();
                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", BasicAuthToken);
                var result = client.PostAsync(Url, GetBody(message)).Result;
                result.EnsureSuccessStatusCode();
            }
        }
    }
}