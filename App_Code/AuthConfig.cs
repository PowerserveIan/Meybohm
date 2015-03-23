using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DotNetOpenAuth.AspNet;
using DotNetOpenAuth.AspNet.Clients;

public static class AuthConfig
{
	private static readonly List<IAuthenticationClient> _clients = new List<IAuthenticationClient>();
	public const string ProviderQueryString = "__provider__";
	public static string ExternalLoginUrl { get { return BaseCode.Helpers.RootPath + "external-login.aspx"; } }
	public const string NoEmailSuppliedAddress = "not_supplied@example.com";

	public static void RegisterOpenAuth()
	{
		//https://dev.twitter.com/apps/
		//Need to enable Read and Write access to the application and specify a callback URL (the actual url is not important, just make it the same as the website url)
		//_clients.Add(new TwitterClient(
		//	consumerKey: "srg3qVIwLJ8YoTywfsNg",
		//	consumerSecret: "mMeIKwmfxTJFqSiJ5vMT5asWHaWa83qDVGKwH046le0"));

		//https://developers.facebook.com/apps
		_clients.Add(new FacebookClient(
			appId: "483495751690628",
			appSecret: "1c5a003f29e37edc91bca5cb4c88a6cd"));

		//_clients.Add(new GoogleOpenIdClient());
		
		//https://manage.dev.live.com/Applications/Index
		//_clients.Add(new MicrosoftClient(
		//	appId: "000000004C0E0474",
		//	appSecret: "35OShgkocW-D-5J6XiQBVzGTFm94JOdK"));

		//_clients.Add(new YahooOpenIdClient());
	}

	public static List<IAuthenticationClient> GetClients()
	{
		return _clients;
	}

	public static IAuthenticationClient GetClientByProviderName(string providerName)
	{
		return _clients.Find(c => c.ProviderName == providerName);
	}
}