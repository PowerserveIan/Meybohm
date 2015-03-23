using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration.Provider;
using System.Globalization;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Security;
using BaseCode;
using Classes.Media352_MembershipProvider;

/// <summary>
/// Recreation of the SqlMembershipProvider using the generated business layer.
/// </summary>
public class Media352_MembershipProvider : MembershipProvider
{
	private string m_AppName;
	private bool m_EnablePasswordReset;
	private bool m_EnablePasswordRetrieval;
	private int m_MaxInvalidPasswordAttempts;
	private int m_MinRequiredNonalphanumericCharacters;
	private int m_MinRequiredPasswordLength;
	private int m_PasswordAttemptWindow;
	private MembershipPasswordFormat m_PasswordFormat;
	private string m_PasswordStrengthRegularExpression;
	private bool m_RequiresQuestionAndAnswer;
	private bool m_RequiresUniqueEmail;

	private static char[] Punctuations
	{
		get { return ("!@#$%^&*").ToCharArray(); }
	}

	private static char[] StartingChars
	{
		get { return ("-").ToCharArray(); }
	}

	public Application application
	{
		get
		{
			List<Application> app = Application.ApplicationGetByName(ApplicationName);
			if (app.Count == 1)
				return app[0];
			throw new Exception("Application not found");
		}
	}

	public override string ApplicationName
	{
		get { return m_AppName ?? "/"; }
		set
		{
			if (string.IsNullOrEmpty(value))
				throw new Exception("ApplicationName is null or empty");
			if (value.Length > 256)
				throw new ProviderException("Provider application name is too long");

			m_AppName = value;
		}
	}

	public override bool EnablePasswordReset
	{
		get { return m_EnablePasswordReset; }
	}

	public override bool EnablePasswordRetrieval
	{
		get { return m_EnablePasswordRetrieval; }
	}

	public override void Initialize(string name, NameValueCollection config)
	{
		if (config == null)
		{
			throw new ArgumentNullException("config");
		}
		if (string.IsNullOrEmpty(name))
		{
			name = "SqlMembershipProvider";
		}
		if (string.IsNullOrEmpty(config["description"]))
		{
			config.Remove("description");
			config.Add("description", "Media352_RoleProvider");
		}
		base.Initialize(name, config);
		//_SchemaVersionCheck = 0;
		m_EnablePasswordRetrieval = GetBooleanValue(config, "enablePasswordRetrieval", false);
		m_EnablePasswordReset = GetBooleanValue(config, "enablePasswordReset", true);
		m_RequiresQuestionAndAnswer = Settings.SecurityQuestionRequired;
		m_RequiresUniqueEmail = GetBooleanValue(config, "requiresUniqueEmail", true);
		m_MaxInvalidPasswordAttempts = GetIntValue(config, "maxInvalidPasswordAttempts", 5, false, 0);
		m_PasswordAttemptWindow = GetIntValue(config, "passwordAttemptWindow", 10, false, 0);
		m_MinRequiredPasswordLength = GetIntValue(config, "minRequiredPasswordLength", 7, false, 0x80);
		m_MinRequiredNonalphanumericCharacters = GetIntValue(config, "minRequiredNonalphanumericCharacters", 1, true, 0x80);
		m_PasswordStrengthRegularExpression = config["passwordStrengthRegularExpression"];
		if (m_PasswordStrengthRegularExpression != null)
		{
			m_PasswordStrengthRegularExpression = m_PasswordStrengthRegularExpression.Trim();
			if (m_PasswordStrengthRegularExpression.Length == 0)
			{
				goto Label_016C;
			}
			try
			{
				new Regex(m_PasswordStrengthRegularExpression);
				goto Label_016C;
			}
			catch (ArgumentException exception1)
			{
				throw new ProviderException(exception1.Message, exception1);
			}
		}
		m_PasswordStrengthRegularExpression = string.Empty;
	Label_016C:
		if (m_MinRequiredNonalphanumericCharacters > m_MinRequiredPasswordLength)
			throw new HttpException("MinRequiredNonalphanumericCharacters_can_not_be_more_than_MinRequiredPasswordLength");

		m_AppName = config["applicationName"];
		if (string.IsNullOrEmpty(m_AppName))
			m_AppName = GetDefaultAppName();

		if (m_AppName.Length > 100)
			throw new ProviderException("Provider_application_name_too_long");

		string text1 = config["passwordFormat"] ?? "Hashed";
		string text4 = text1;

		switch (text4)
		{
			case "Clear":
				m_PasswordFormat = MembershipPasswordFormat.Clear;
				break;
			case "Encrypted":
				m_PasswordFormat = MembershipPasswordFormat.Encrypted;
				break;
			case "Hashed":
				m_PasswordFormat = MembershipPasswordFormat.Hashed;
				break;
			default:
				throw new ProviderException("Provider_bad_password_format");
		}

		string text2 = config["connectionStringName"];
		if (String.IsNullOrEmpty(text2))
			throw new ProviderException("Connection_name_not_specified");

		config.Remove("connectionStringName");
		config.Remove("enablePasswordRetrieval");
		config.Remove("enablePasswordReset");
		config.Remove("applicationName");
		config.Remove("requiresUniqueEmail");
		config.Remove("maxInvalidPasswordAttempts");
		config.Remove("passwordAttemptWindow");
		config.Remove("commandTimeout");
		config.Remove("passwordFormat");
		config.Remove("name");
		config.Remove("minRequiredPasswordLength");
		config.Remove("minRequiredNonalphanumericCharacters");
		config.Remove("passwordStrengthRegularExpression");
		if (config.Count > 0)
		{
			string text3 = config.GetKey(0);
			if (!string.IsNullOrEmpty(text3))
				throw new ProviderException("Provider_unrecognized_attribute");
		}
	}

	public override bool ChangePassword(string username, string oldPassword, string newPassword)
	{
		int tempPasswordFormat;
		string salt;

		CheckParameter(ref newPassword, true, true, false, 80, "new Password");

		if (!CheckPassword(username, oldPassword, false, out salt, out tempPasswordFormat))
			return false;

		if (newPassword.Length < MinRequiredPasswordLength)
			throw new ArgumentException("New password is too short");

		int nonAlphaNumeric = 0;
		for (int counter = 0; counter < newPassword.Length; counter++)
		{
			if (!char.IsLetterOrDigit(newPassword, counter))
				nonAlphaNumeric++;
		}

		if (nonAlphaNumeric < MinRequiredNonAlphanumericCharacters)
			throw new ArgumentException("New password needs more non alpha numeric characters");

		if ((PasswordStrengthRegularExpression.Length > 0) && !Regex.IsMatch(newPassword, PasswordStrengthRegularExpression))
			throw new ArgumentException("New password does not match regular expression");

		string encodedPassword = EncodePassword(newPassword, tempPasswordFormat, salt);
		if (encodedPassword.Length > 80)
			throw new ArgumentException("New password is too long");

		ValidatePasswordEventArgs args = new ValidatePasswordEventArgs(username, newPassword, false);
		OnValidatingPassword(args);
		if (args.Cancel)
		{
			if (args.FailureInformation != null)
				throw args.FailureInformation;
			throw new ArgumentException("Membership custom new password validation failure");
		}
		List<User> users = User.UserGetByName(username);
		User user = null;
		foreach (User tmpUser in users)
		{
			if (tmpUser.ApplicationID == application.ApplicationID)
				user = tmpUser;
		}
		if (user == null)
			throw new Exception();

		user.ChangePasswordID = null;
		user.LastActivity = DateTime.UtcNow;
		user.Password = encodedPassword;
		user.Salt = salt;
		user.PasswordFormat = tempPasswordFormat;
		user.LastPasswordChange = DateTime.UtcNow;
		user.FailedAnswerAttempts = 0;
		user.FailedPasswordAttempts = 0;
		user.Save();

		return true;
	}

	public override bool ChangePasswordQuestionAndAnswer(string username, string password, string newPasswordQuestion,
														 string newPasswordAnswer)
	{
		string salt;
		int tempPasswordFormat;

		if (!CheckPassword(username, password, false, out salt, out tempPasswordFormat))
			return false;

		CheckParameter(ref newPasswordQuestion, RequiresQuestionAndAnswer, RequiresQuestionAndAnswer, false, 50,
					   "New password question");

		if (newPasswordAnswer != null)
			newPasswordAnswer = newPasswordAnswer.Trim();

		string encodedPasswordAnswer = !string.IsNullOrEmpty(newPasswordAnswer)
										? EncodePassword(newPasswordAnswer.ToLower(CultureInfo.InvariantCulture),
														 tempPasswordFormat, salt)
										: newPasswordAnswer;

		CheckParameter(ref encodedPasswordAnswer, RequiresQuestionAndAnswer, RequiresQuestionAndAnswer, false, 80,
					   "New password answer");

		List<User> users = User.UserGetByName(username);
		User user = null;
		foreach (User tmpUser in users)
		{
			if (tmpUser.ApplicationID == application.ApplicationID)
				user = tmpUser;
		}
		if (user == null)
			throw new Exception();
		user.PasswordQuestion = newPasswordQuestion;
		user.PasswordAnswer = encodedPasswordAnswer;
		user.Save();
		return true;
	}

	public override MembershipUser CreateUser(string username, string password, string email, string passwordQuestion,
											  string passwordAnswer, bool isApproved, object providerUserKey,
											  out MembershipCreateStatus status)
	{
		if (!String.IsNullOrEmpty(passwordAnswer))
			passwordAnswer = passwordAnswer.Trim();
		string encodedPasswordAnswer = passwordAnswer;

		string salt = GenerateSalt();
		string encodedPassword = EncodePassword(password, (int)PasswordFormat, salt);

		int nonAlphaNumeric = 0;
		for (int counter2 = 0; counter2 < password.Length; counter2++)
		{
			if (!char.IsLetterOrDigit(password, counter2))
			{
				nonAlphaNumeric++;
			}
		}

		if (password.Length < MinRequiredPasswordLength
			|| !ValidateParameter(ref encodedPassword, true, true, false, 250)
			|| nonAlphaNumeric < MinRequiredNonAlphanumericCharacters
			||
			(!String.IsNullOrEmpty(PasswordStrengthRegularExpression) &&
			 !Regex.IsMatch(password, PasswordStrengthRegularExpression)))
		{
			status = MembershipCreateStatus.InvalidPassword;
			return null;
		}

		//We can't unhash so use encryption instead for security question answers
		int securityAnswerPWFormat = m_PasswordFormat == MembershipPasswordFormat.Hashed ? (int)MembershipPasswordFormat.Encrypted : (int)m_PasswordFormat;
		if (Settings.SecurityQuestionRequired && (!(HttpContext.Current.Handler is System.Web.UI.Page) || (((System.Web.UI.Page)HttpContext.Current.Handler).Items["SecurityQuestionRequired"] == null || Convert.ToBoolean(((System.Web.UI.Page)HttpContext.Current.Handler).Items["SecurityQuestionRequired"]))))
		{
			if (!String.IsNullOrEmpty(passwordAnswer))
				encodedPasswordAnswer = EncodePassword(passwordAnswer.ToLower(CultureInfo.InvariantCulture), (int)securityAnswerPWFormat,
													   salt);

			if (!ValidateParameter(ref encodedPasswordAnswer, RequiresQuestionAndAnswer, true, false, 255))
			{
				status = MembershipCreateStatus.InvalidAnswer;
				return null;
			}

			if (!ValidateParameter(ref passwordQuestion, RequiresQuestionAndAnswer, true, false, 50))
			{
				status = MembershipCreateStatus.InvalidQuestion;
				return null;
			}
		}

		if (!ValidateParameter(ref username, true, true, true, 50))
		{
			status = MembershipCreateStatus.InvalidUserName;
			return null;
		}

		if (!ValidateParameter(ref email, RequiresUniqueEmail, RequiresUniqueEmail, false, 382))
		{
			status = MembershipCreateStatus.InvalidEmail;
			return null;
		}

		if ((providerUserKey != null) && !(providerUserKey is Int32))
		{
			status = MembershipCreateStatus.InvalidProviderUserKey;
			return null;
		}

		ValidatePasswordEventArgs args = new ValidatePasswordEventArgs(username, password, true);
		OnValidatingPassword(args);
		if (args.Cancel)
		{
			status = MembershipCreateStatus.InvalidPassword;
			return null;
		}

		//Check email and username uniqueness
		List<User> usersToCheck = User.UserGetByName(username);
		foreach (User tmpUser in usersToCheck)
			if (tmpUser.ApplicationID == application.ApplicationID)
			{
				status = MembershipCreateStatus.DuplicateUserName;
				return null;
			}

		if (RequiresUniqueEmail)
		{
			usersToCheck = User.UserGetByEmail(email);

			foreach (User tmpUser in usersToCheck)
				if (tmpUser.ApplicationID == application.ApplicationID)
				{
					status = MembershipCreateStatus.DuplicateEmail;
					return null;
				}
		}

		User user1 = new User();
		user1.ApplicationID = application.ApplicationID;
		if (!isApproved)
			user1.ChangePasswordID = Guid.NewGuid();
		user1.Created = DateTime.UtcNow;
		user1.Email = email;
		user1.FailedAnswerAttempts = 0;
		user1.FailedPasswordAttempts = 0;
		user1.IsApproved = isApproved;
		user1.LastActivity = DateTime.UtcNow;
		user1.LastLockout = null;
		user1.LastLogin = DateTime.UtcNow;
		user1.LastPasswordChange = DateTime.UtcNow;
		user1.Name = username;
		user1.Password = encodedPassword;
		user1.PasswordFormat = (int)PasswordFormat;
		if (Settings.SecurityQuestionRequired)
		{
			user1.PasswordAnswer = encodedPasswordAnswer;
			user1.PasswordQuestion = passwordQuestion;
		}
		user1.Salt = salt;
		user1.UniqueEmail = RequiresUniqueEmail ? 1 : 0;
		user1.Save();

		//Added to set the provider user key to the pkey field since it is not a guid being passed in
		providerUserKey = user1.UserID;

		MembershipUser newUser = new MembershipUser("Media352_MembershipProvider", user1.Name, providerUserKey, user1.Email,
													user1.PasswordQuestion, null, user1.IsApproved, false, user1.Created,
													user1.Created, user1.Created, user1.Created, user1.Created);
		status = MembershipCreateStatus.Success;
		return newUser;
	}

	public override bool DeleteUser(string username, bool deleteAllRelatedData)
	{
		List<User> users = User.UserGetByName(username);
		User user = null;
		foreach (User tmpUser in users)
		{
			if (tmpUser.ApplicationID == application.ApplicationID)
				user = tmpUser;
		}
		if (user == null)
			throw new Exception("User not found");
		user.Deleted = true;
		user.Save();
		return true;
	}

	public override MembershipUserCollection FindUsersByEmail(string emailToMatch, int pageIndex, int pageSize,
															  out int totalRecords)
	{
		MembershipUserCollection members = new MembershipUserCollection();
		CheckParameter(ref emailToMatch, false, false, false, 382, "email to match");
		if (pageIndex < 0)
			throw new ArgumentException("Bad page index");
		if (pageSize < 1)
			throw new ArgumentException("Bad page size");
		long page = ((pageIndex * pageSize) + pageSize) - 1;
		if (page > 0x7fffffff)
			throw new ArgumentException("Page index and page size are invalid");
		List<User> users = User.UserPage(pageIndex, pageSize, emailToMatch, "Email", true);
		totalRecords = users.Count;
		foreach (User tmpUser in users)
		{
			if (tmpUser.ApplicationID == application.ApplicationID)
				members.Add(new MembershipUser(Name, tmpUser.Name, tmpUser.UserID, tmpUser.Email, tmpUser.PasswordQuestion, "",
											   tmpUser.IsApproved, tmpUser.Locked,
											   tmpUser.Created, tmpUser.LastLogin, tmpUser.LastActivity, tmpUser.LastPasswordChange,
											   tmpUser.LastLockout.HasValue ? tmpUser.LastLockout.Value : (new DateTime())));
		}
		return members;
	}

	public override MembershipUserCollection FindUsersByName(string usernameToMatch, int pageIndex, int pageSize,
															 out int totalRecords)
	{
		MembershipUserCollection members = new MembershipUserCollection();
		CheckParameter(ref usernameToMatch, false, false, false, 50, "username to match");
		if (pageIndex < 0)
			throw new ArgumentException("Bad page index");
		if (pageSize < 1)
			throw new ArgumentException("Bad page size");
		long page = ((pageIndex * pageSize) + pageSize) - 1;
		if (page > 0x7fffffff)
			throw new ArgumentException("Page index and page size are invalid");
		List<User> users = User.UserPage(pageIndex, pageSize, usernameToMatch, "Name", true);
		totalRecords = users.Count;
		foreach (User tmpUser in users)
		{
			if (tmpUser.ApplicationID == application.ApplicationID)
				members.Add(new MembershipUser(Name, tmpUser.Name, tmpUser.UserID, tmpUser.Email, tmpUser.PasswordQuestion, "",
											   tmpUser.IsApproved, tmpUser.Locked,
											   tmpUser.Created, tmpUser.LastLogin, tmpUser.LastActivity, tmpUser.LastPasswordChange,
											   tmpUser.LastLockout.HasValue ? tmpUser.LastLockout.Value : (new DateTime())));
		}
		return members;
	}

	public override MembershipUserCollection GetAllUsers(int pageIndex, int pageSize, out int totalRecords)
	{
		MembershipUserCollection members = new MembershipUserCollection();
		if (pageIndex < 0)
			throw new ArgumentException("Bad page index");
		if (pageSize < 1)
			throw new ArgumentException("Bad page size");
		long page = ((pageIndex * pageSize) + pageSize) - 1;
		if (page > 0x7fffffff)
			throw new ArgumentException("Page index and page size are invalid");
		List<User> users = User.UserPage(pageIndex, pageSize, "", "Name", true);
		totalRecords = users.Count;
		foreach (User tmpUser in users)
		{
			if (tmpUser.ApplicationID == application.ApplicationID)
				members.Add(new MembershipUser(Name, tmpUser.Name, tmpUser.UserID, tmpUser.Email, tmpUser.PasswordQuestion, "",
											   tmpUser.IsApproved, tmpUser.Locked,
											   tmpUser.Created, tmpUser.LastLogin, tmpUser.LastActivity, tmpUser.LastPasswordChange,
											   tmpUser.LastLockout.HasValue ? tmpUser.LastLockout.Value : (new DateTime())));
		}
		return members;
	}

	public override int GetNumberOfUsersOnline()
	{
		return User.UserGetByOnline(true).Count;
	}

	public override string GetPassword(string username, string answer)
	{
		if (!EnablePasswordRetrieval)
			throw new NotSupportedException("Membership password retreival not supported");

		string encodedPassword = GetEncodedPasswordAnswer(username, answer);

		int tempPasswordFormat;
		int status;
		string password = GetPasswordFromDB(username, encodedPassword, RequiresQuestionAndAnswer, out tempPasswordFormat,
											out status);
		if (password == null)
			throw new MembershipPasswordException("Invalid Username or Security Question Answer");

		return UnEncodePassword(password, tempPasswordFormat);
	}

	public override MembershipUser GetUser(string username, bool userIsOnline)
	{
		List<User> users = User.UserGetByName(username);
		User user = null;
		foreach (User tmpUser in users)
		{
			if (tmpUser.ApplicationID == application.ApplicationID)
				user = tmpUser;
		}
		if (user == null)
			return null;

		if (userIsOnline)
		{
			user.LastActivity = DateTime.UtcNow;
			user.Save();
		}
		return new MembershipUser("Media352_MembershipProvider", user.Name, user.UserID, user.Email, user.PasswordQuestion, "",
								  user.IsApproved, user.Locked,
								  user.Created, user.LastLogin, user.LastActivity, user.LastPasswordChange,
								  user.LastLockout.HasValue ? user.LastLockout.Value : (new DateTime()));
	}

	public override MembershipUser GetUser(object providerUserKey, bool userIsOnline)
	{
		if (providerUserKey == null)
			throw new ArgumentNullException("providerUserKey");
		if (!(providerUserKey is Int32))
			throw new ArgumentException("invalid user provider key");

		User user = User.GetByID((int)providerUserKey);
		if (user.ApplicationID != application.ApplicationID)
			return null;

		if (userIsOnline)
		{
			user.LastActivity = DateTime.UtcNow;
			user.Save();
		}
		return new MembershipUser("Media352_MembershipProvider", user.Name, user.UserID, user.Email, user.PasswordQuestion, "",
								  user.IsApproved, user.Locked,
								  user.Created, user.LastLogin, user.LastActivity, user.LastPasswordChange,
								  user.LastLockout.HasValue ? user.LastLockout.Value : (new DateTime()));
	}

	public override string GetUserNameByEmail(string email)
	{
		List<User> users = User.UserGetByEmail(email);
		User user = null;
		foreach (User tmpUser in users)
		{
			if (tmpUser.ApplicationID == application.ApplicationID)
				user = tmpUser;
		}
		if (user == null)
			throw new NoUserFoundException("user not found");
		return user.Name;
	}

	public override string ResetPassword(string username, string answer)
	{
		string password;
		int status;
		string salt;
		int tempPasswordFormat;
		int failedPasswordAttemptCount;
		int failedPasswordAnswerAttemptCount;
		bool isApproved;
		DateTime lastLogin;
		DateTime lastActivity;

		if (EnablePasswordReset == false)
			throw new NotSupportedException("Not configured to support password resets");

		GetPasswordWithFormat(username, out status, out password, out tempPasswordFormat, out salt,
							  out failedPasswordAttemptCount, out failedPasswordAnswerAttemptCount, out isApproved,
							  out lastLogin, out lastActivity);
		if (status == 0)
		{
			if (answer != null)
				answer = answer.Trim();

			int securityAnswerPWFormat = tempPasswordFormat == (int)MembershipPasswordFormat.Hashed ? (int)MembershipPasswordFormat.Encrypted : (int)tempPasswordFormat;
			string encodedAnswer = !String.IsNullOrEmpty(answer)
									? EncodePassword(answer.ToLower(CultureInfo.InvariantCulture), securityAnswerPWFormat, salt)
									: answer;
			CheckParameter(ref encodedAnswer, RequiresQuestionAndAnswer, RequiresQuestionAndAnswer, false, 255, "password answer");
			string unencryptedPassword = GeneratePassword();
			string newPassword = EncodePassword(unencryptedPassword, tempPasswordFormat, salt);
			ValidatePasswordEventArgs args = new ValidatePasswordEventArgs(username, newPassword, false);
			OnValidatingPassword(args);
			if (args.Cancel)
			{
				if (args.FailureInformation != null)
					throw args.FailureInformation;
				throw new ProviderException("Membership custom password validation failure");
			}

			List<User> users = User.UserGetByName(username);
			User user = null;
			foreach (User tmpUser in users)
			{
				if (tmpUser.ApplicationID == application.ApplicationID)
					user = tmpUser;
			}
			if (user == null)
				throw new Exception("user not found");
			if (RequiresQuestionAndAnswer && !String.IsNullOrEmpty(user.PasswordAnswer) && !user.PasswordAnswer.Equals(encodedAnswer, StringComparison.OrdinalIgnoreCase))
			{
				user.FailedAnswerAttempts++;
				user.Save();
				throw new MembershipPasswordException("invalid password answer");
			}
			user.LastActivity = DateTime.UtcNow;
			user.Password = newPassword;
			user.Salt = salt;
			user.PasswordFormat = tempPasswordFormat;
			user.LastPasswordChange = DateTime.UtcNow;
			user.FailedAnswerAttempts = 0;
			user.FailedPasswordAttempts = 0;
			user.Save();

			return unencryptedPassword;
		}
		return "";
	}

	public override bool UnlockUser(string userName)
	{
		List<User> users = User.UserGetByName(userName);
		User user = null;
		foreach (User tmpUser in users)
		{
			if (tmpUser.ApplicationID == application.ApplicationID)
				user = tmpUser;
		}
		if (user == null)
			throw new Exception("user not found");
		user.Locked = false;
		user.FailedPasswordAttempts = 0;
		user.FailedAnswerAttempts = 0;
		user.Save();
		return true;
	}

	public override void UpdateUser(MembershipUser user2)
	{
		List<User> users = User.UserGetByName(user2.UserName);
		User user = null;
		foreach (User tmpUser in users)
		{
			if (tmpUser.ApplicationID == application.ApplicationID)
				user = tmpUser;
		}
		if (user == null)
			throw new Exception("user not found");
		user.Email = user2.Email;
		user.IsApproved = user2.IsApproved;
		user.LastLogin = user2.LastLoginDate;
		user.LastActivity = user2.LastActivityDate;
		user.UniqueEmail = RequiresUniqueEmail ? 1 : 0;
		user.Save();
	}

	public override bool ValidateUser(string username, string password)
	{
		if ((ValidateParameter(ref username, true, true, true, 50) && ValidateParameter(ref password, true, true, false, 250)) &&
			CheckPassword(username, password, true))
			return true;
		return false;
	}

	#region imported methods from static internal methods

	/// <summary>
	/// Exported from SecUtility
	/// </summary>
	/// <param name="param"></param>
	/// <param name="checkForNull"></param>
	/// <param name="checkIfEmpty"></param>
	/// <param name="checkForCommas"></param>
	/// <param name="maxSize"></param>
	/// <returns></returns>
	internal static bool ValidateParameter(ref string param, bool checkForNull, bool checkIfEmpty, bool checkForCommas,
										   int maxSize)
	{
		if (param == null)
			return !checkForNull;

		param = param.Trim();
		return ((!checkIfEmpty || param.Length >= 1) && (maxSize <= 0 || param.Length <= maxSize)) &&
			   (!checkForCommas || !param.Contains(","));
	}

	internal string GenerateSalt()
	{
		return BCrypt.Net.BCrypt.GenerateSalt();
	}

	internal string EncodePassword(string pass, int passwordFormat, string salt)
	{
		if (passwordFormat == (int)MembershipPasswordFormat.Clear)
			return pass;
		if (passwordFormat == (int)MembershipPasswordFormat.Hashed)
			return BCrypt.Net.BCrypt.HashPassword(pass, salt);
		return BaseCode.Helpers.CryptorRijndael.Encrypt(pass, salt);
	}

	internal static void CheckParameter(ref string param, bool checkForNull, bool checkIfEmpty, bool checkForCommas,
										int maxSize, string paramName)
	{
		if (param == null)
		{
			if (checkForNull)
				throw new ArgumentNullException(paramName);
		}
		else
		{
			param = param.Trim();

			if (checkIfEmpty && String.IsNullOrEmpty(param))
				throw new ArgumentException(paramName + " cannot be empty");

			if (maxSize > 0 && param.Length > maxSize)
				throw new ArgumentException(paramName + " is too long");

			if (checkForCommas && param.Contains(","))
				throw new ArgumentException(paramName + " contains ','");
		}
	}

	private void GetPasswordWithFormat(string username, out int status, out string password, out int passwordFormat,
									   out string passwordSalt, out int failedPasswordAttemptCount,
									   out int failedPasswordAnswerAttemptCount, out bool isApproved,
									   out DateTime lastLoginDate, out DateTime lastActivityDate)
	{
		User user = null;
		List<User> users = User.UserGetByName(username);
		foreach (User tmpUser in users)
		{
			if (tmpUser.ApplicationID == application.ApplicationID)
			{
				//user has been found
				user = tmpUser;
			}
		}

		if (user != null)
		{
			user.LastActivity = DateTime.UtcNow;
			user.Save();

			password = user.Password;
			passwordFormat = user.PasswordFormat;
			passwordSalt = user.Salt;
			failedPasswordAttemptCount = user.FailedPasswordAttempts;
			failedPasswordAnswerAttemptCount = user.FailedAnswerAttempts;
			isApproved = user.IsApproved;
			lastLoginDate = user.LastLogin;
			lastActivityDate = user.LastActivity;
			status = 0;
		}
		else
		{
			password = null;
			passwordFormat = 0;
			passwordSalt = null;
			failedPasswordAttemptCount = 0;
			failedPasswordAnswerAttemptCount = 0;
			isApproved = false;
			lastLoginDate = DateTime.UtcNow;
			lastActivityDate = DateTime.UtcNow;
			status = 1;
		}
	}

	private bool CheckPassword(string username, string password, bool failIfNotApproved, out string salt,
							   out int passwordFormat)
	{
		string encodedPassword;
		int status;
		int failedPasswordAttemptCount;
		int failedPasswordAnswerAttemptCount;
		bool isApproved;
		DateTime lastLoginDate;
		DateTime lastActivityDate;

		GetPasswordWithFormat(username, out status, out encodedPassword, out passwordFormat, out salt,
							  out failedPasswordAttemptCount, out failedPasswordAnswerAttemptCount, out isApproved,
							  out lastLoginDate, out lastActivityDate);

		if (status != 0)
			return false;

		if (!isApproved && failIfNotApproved)
			throw new MemberAccessException("Your account has not been approved yet");

		bool passwordMatch = encodedPassword.Equals(EncodePassword(password, passwordFormat, salt));
		if (!passwordMatch && passwordFormat == 2)
		{
			//Fix for older sites with existing users using the old encryption method
			byte[] buffer1 = Convert.FromBase64String(encodedPassword);
			byte[] buffer2 = DecryptPassword(buffer1);


			if (buffer2 != null)
				encodedPassword = Encoding.Unicode.GetString(buffer2, 0x10, buffer2.Length - 0x10);
			passwordMatch = password.Equals(encodedPassword, StringComparison.OrdinalIgnoreCase);
		}

		//now we have to get the user to check the password attempt frequency
		List<User> users = User.UserGetByName(username);
		User user = null;
		foreach (User tmpUser in users)
		{
			if (tmpUser.ApplicationID == application.ApplicationID)
			{
				user = tmpUser;
				break;
			}
		}

		if (user == null || user.Locked)
			throw new MemberAccessException("Your account has been locked, please contact the administrator to unlock it.");

		if (MaxInvalidPasswordAttempts >= user.FailedPasswordAttempts && passwordMatch)
		{
			user.FailedPasswordAttempts = 0;
			user.Save();
			return true;
		}

		if (isApproved && failedPasswordAttemptCount == 0 && failedPasswordAnswerAttemptCount == 0 && passwordMatch)
			return true;

		user.FailedPasswordAttempts++;
		if (user.FailedPasswordAttempts >= MaxInvalidPasswordAttempts)
			user.Locked = true;
		user.Save();
		return false;
	}

	private string GetEncodedPasswordAnswer(string username, string passwordAnswer)
	{
		if (passwordAnswer != null)
			passwordAnswer = passwordAnswer.Trim();

		if (!String.IsNullOrEmpty(passwordAnswer))
		{
			User userEntity = User.UserGetByName(username).FirstOrDefault();
			if (userEntity == null)
				throw new ProviderException("Error getting password");

			int securityAnswerPWFormat = userEntity.PasswordFormat == (int)MembershipPasswordFormat.Hashed ? (int)MembershipPasswordFormat.Encrypted : (int)userEntity.PasswordFormat;
			return EncodePassword(passwordAnswer.ToLower(CultureInfo.InvariantCulture), securityAnswerPWFormat,
								  userEntity.Salt);
		}
		return passwordAnswer;
	}

	private string GetPasswordFromDB(string username, string passwordAnswer, bool requiresQuestionAndAnswer,
									 out int passwordFormat, out int status)
	{
		string password;

		List<User> users = User.UserGetByName(username);
		User user = null;
		foreach (User tmpUser in users)
		{
			if (tmpUser.ApplicationID == application.ApplicationID)
				user = tmpUser;
		}
		if (user == null)
		{
			passwordFormat = 0;
			status = -1;
			return null;
		}
		status = 0;
		if (user.Deleted)
			status = 1;
		if (user.Locked)
			status = 99;
		if (requiresQuestionAndAnswer)
		{
			if (!passwordAnswer.Equals(user.PasswordAnswer, StringComparison.OrdinalIgnoreCase))
			{
				user.FailedAnswerAttempts++;
				if (user.FailedAnswerAttempts >= MaxInvalidPasswordAttempts)
				{
					user.Locked = true;
					if (!user.LastLockout.HasValue || user.LastLockout.Value.AddMinutes(PasswordAttemptWindow) < DateTime.UtcNow)
						user.LastLockout = DateTime.UtcNow;
				}

				user.Save();
				status = 3;
			}
			else if (user.FailedAnswerAttempts > 0)
			{
				user.FailedAnswerAttempts = 0;
				user.Save();
			}
		}
		if (status == 0)
		{
			password = user.Password;
			passwordFormat = user.PasswordFormat;
		}
		else
		{
			password = null;
			passwordFormat = 0;
		}

		return password;
	}

	public string GetUnencodedPassword(string pass, int passwordFormat)
	{
		return UnEncodePassword(pass, passwordFormat);
	}

	public string GetUnencodedPassword(string pass, int passwordFormat, string salt)
	{
		return UnEncodePassword(pass, passwordFormat, salt);
	}

	internal string UnEncodePassword(string pass, int passwordFormat, string salt = "")
	{
		switch (passwordFormat)
		{
			case 0:
				return pass;
			case 1:
				throw new ProviderException("Provider can not decode hashed password");
		}
		try
		{
			if (!String.IsNullOrEmpty(salt))
				return Helpers.CryptorRijndael.Decrypt(pass, salt);
			return Helpers.CryptorRijndael.Decrypt(pass);
		}
		catch (Exception)
		{
			//Fix for older sites with existing users using the old encryption method
			byte[] buffer1 = Convert.FromBase64String(pass);
			byte[] buffer2 = DecryptPassword(buffer1);

			if (buffer2 == null)
				return null;
			return Encoding.Unicode.GetString(buffer2, 0x10, buffer2.Length - 0x10);
		}
	}

	public virtual string GeneratePassword()
	{
		return GeneratePassword((MinRequiredPasswordLength < 14) ? 14 : MinRequiredPasswordLength,
								MinRequiredNonAlphanumericCharacters);
	}

	public static string GeneratePassword(int length, int numberOfNonAlphanumericCharacters)
	{
		if (length < 1 || length > 250)
			throw new ArgumentException("Membership password length incorrect");

		if (numberOfNonAlphanumericCharacters > length || numberOfNonAlphanumericCharacters < 0)
			throw new ArgumentException("Membership min required non alphanumeric characters incorrect");

		while (true)
		{
			int num1;
			byte[] buffer1 = new byte[length];
			char[] chArray1 = new char[length];
			int num2 = 0;
			new RNGCryptoServiceProvider().GetBytes(buffer1);
			for (int num3 = 0; num3 < length; num3++)
			{
				int num4 = buffer1[num3] % 87;
				if (num4 < 10)
					chArray1[num3] = Convert.ToChar(Convert.ToUInt16(48 + num4));
				else if (num4 < 36)
					chArray1[num3] = Convert.ToChar(Convert.ToUInt16((65 + num4) - 10));
				else if (num4 < 62)
					chArray1[num3] = Convert.ToChar(Convert.ToUInt16((97 + num4) - 36));
				else
				{
					if (num4 - 62 >= Punctuations.Length)
						chArray1[num3] = Punctuations[Punctuations.Length - 1];
					else
						chArray1[num3] = Punctuations[num4 - 62];
					num2++;
				}
			}
			if (num2 < numberOfNonAlphanumericCharacters)
			{
				Random random1 = new Random();
				for (int num5 = 0; num5 < (numberOfNonAlphanumericCharacters - num2); num5++)
				{
					int num6;
					do
					{
						num6 = random1.Next(0, length);
					} while (!char.IsLetterOrDigit(chArray1[num6]));
					chArray1[num6] = Punctuations[random1.Next(0, Punctuations.Length)];
				}
			}
			string text1 = new string(chArray1);

			if (!IsDangerousString(text1, out num1))
				return text1;
		}
	}

	internal static bool IsDangerousString(string s, out int matchIndex)
	{
		matchIndex = 0;
		int num1 = 0;
		while (true)
		{
			int num2 = s.IndexOfAny(StartingChars, num1);
			if (num2 < 0 || num2 == (s.Length - 1))
				return false;

			matchIndex = num2;
			char ch1 = s[num2];
			if (ch1 != '&' && (ch1 == '<') && (IsAtoZ(s[num2 + 1]) || (s[num2 + 1] == '!')))
				return true;

			if (s[num2 + 1] == '#')
				return true;
			num1 = num2 + 1;
		}
	}

	private static bool IsAtoZ(char c)
	{
		if ((c >= 'a') && (c <= 'z'))
			return true;
		return c >= 'A' && c <= 'Z';
	}

	private bool CheckPassword(string username, string password, bool failIfNotApproved)
	{
		string text1;
		int num1;
		return CheckPassword(username, password, failIfNotApproved, out text1, out num1);
	}

	internal static bool GetBooleanValue(NameValueCollection config, string valueName, bool defaultValue)
	{
		bool flag1;
		string text1 = config[valueName];
		if (text1 == null)
			return defaultValue;

		if (!bool.TryParse(text1, out flag1))
			throw new ProviderException("Value_must_be_boolean");
		return flag1;
	}

	internal static int GetIntValue(NameValueCollection config, string valueName, int defaultValue, bool zeroAllowed,
									int maxValueAllowed)
	{
		int num1;
		string text1 = config[valueName];
		if (text1 == null)
			return defaultValue;

		if (!int.TryParse(text1, out num1))
		{
			if (zeroAllowed)
				throw new ProviderException("Value_must_be_non_negative_integer");
			throw new ProviderException("Value_must_be_positive_integer");
		}
		if (zeroAllowed && (num1 < 0))
			throw new ProviderException("Value_must_be_non_negative_integer");
		if (!zeroAllowed && (num1 <= 0))
			throw new ProviderException("Value_must_be_positive_integer");
		if ((maxValueAllowed > 0) && (num1 > maxValueAllowed))
			throw new ProviderException("Value_too_big");
		return num1;
	}

	internal static string GetDefaultAppName()
	{
		return "/";
	}

	#endregion

	#region Added Custom For updating username, password, and question/answer

	/// <summary>
	/// Use this method if you don't know the old password but still need to change the password
	/// </summary>
	public bool ChangePassword(string username, ref string oldPassword, string newPassword)
	{
		int status;
		string salt;
		int tempPasswordFormat;
		int failedPasswordAttemptCount;
		int failedPasswordAnswerAttemptCount;
		bool isApproved;
		DateTime lastLogin;
		DateTime lastActivity;

		GetPasswordWithFormat(username, out status, out oldPassword, out tempPasswordFormat, out salt,
							  out failedPasswordAttemptCount, out failedPasswordAnswerAttemptCount, out isApproved,
							  out lastLogin, out lastActivity);
		if (status == 0)
		{
			if (newPassword.Length < MinRequiredPasswordLength)
				throw new ArgumentException("New password is too short");

			int nonAlphaNumeric = 0;
			for (int counter = 0; counter < newPassword.Length; counter++)
			{
				if (!char.IsLetterOrDigit(newPassword, counter))
					nonAlphaNumeric++;
			}
			if (nonAlphaNumeric < MinRequiredNonAlphanumericCharacters)
				throw new ArgumentException("New password needs more non alpha numeric characters");

			if ((PasswordStrengthRegularExpression.Length > 0) && !Regex.IsMatch(newPassword, PasswordStrengthRegularExpression))
				throw new ArgumentException("New password does not match regular expression");

			string encodedPassword = EncodePassword(newPassword, tempPasswordFormat, salt);
			if (encodedPassword.Length > 250)
				throw new ArgumentException("New password is too long");

			ValidatePasswordEventArgs args = new ValidatePasswordEventArgs(username, newPassword, false);
			OnValidatingPassword(args);
			if (args.Cancel)
			{
				if (args.FailureInformation != null)
					throw args.FailureInformation;
				throw new ArgumentException("Membership custom new password validation failure");
			}
			List<User> users = User.UserGetByName(username);
			User user = null;
			foreach (User tmpUser in users)
			{
				if (tmpUser.ApplicationID == application.ApplicationID)
					user = tmpUser;
			}
			if (user == null)
				throw new Exception();

			user.ChangePasswordID = null;
			user.LastActivity = DateTime.UtcNow;
			user.Password = encodedPassword;
			user.Salt = salt;
			user.PasswordFormat = tempPasswordFormat;
			user.LastPasswordChange = DateTime.UtcNow;
			user.FailedAnswerAttempts = 0;
			user.FailedPasswordAttempts = 0;
			user.Save();

			return true;
		}
		return false;
	}

	public bool UpdateUserNameAndPasswordRelatedInfo(object providerUserKey, string username, string password,
													 string passwordQuestion, string passwordAnswer,
													 out MembershipCreateStatus status)
	{
		if (passwordAnswer != null)
			passwordAnswer = passwordAnswer.Trim();
		string encodedPasswordAnswer = passwordAnswer;

		string salt = GenerateSalt();
		string encodedPassword = EncodePassword(password, (int)PasswordFormat, salt);

		int nonAlphaNumeric = 0;
		for (int counter2 = 0; counter2 < password.Length; counter2++)
		{
			if (!char.IsLetterOrDigit(password, counter2))
			{
				nonAlphaNumeric++;
			}
		}

		if (!ValidateParameter(ref encodedPassword, true, true, false, 250)
			|| password.Length < MinRequiredPasswordLength
			|| nonAlphaNumeric < MinRequiredNonAlphanumericCharacters
			||
			(PasswordStrengthRegularExpression != null && PasswordStrengthRegularExpression.Length > 0 &&
			 !Regex.IsMatch(password, PasswordStrengthRegularExpression)))
		{
			status = MembershipCreateStatus.InvalidPassword;
			return false;
		}

		if (Settings.SecurityQuestionRequired)
		{
			if (!String.IsNullOrEmpty(passwordAnswer))
			{
				int securityAnswerPWFormat = m_PasswordFormat == MembershipPasswordFormat.Hashed ? (int)MembershipPasswordFormat.Encrypted : (int)m_PasswordFormat;
				encodedPasswordAnswer = EncodePassword(passwordAnswer.ToLower(CultureInfo.InvariantCulture), securityAnswerPWFormat,
													   salt);
			}

			if (!ValidateParameter(ref encodedPasswordAnswer, RequiresQuestionAndAnswer, true, false, 255))
			{
				status = MembershipCreateStatus.InvalidAnswer;
				return false;
			}

			if (!ValidateParameter(ref passwordQuestion, RequiresQuestionAndAnswer, true, false, 50))
			{
				status = MembershipCreateStatus.InvalidQuestion;
				return false;
			}
		}
		if (!ValidateParameter(ref username, true, true, true, 50))
		{
			status = MembershipCreateStatus.InvalidUserName;
			return false;
		}

		ValidatePasswordEventArgs args = new ValidatePasswordEventArgs(username, password, true);
		OnValidatingPassword(args);
		if (args.Cancel)
		{
			status = MembershipCreateStatus.InvalidPassword;
			return false;
		}

		//Check email and username uniqueness
		List<User> usersToCheck = User.UserGetByName(username);
		foreach (User tmpUser in usersToCheck)
		{
			if ((tmpUser.ApplicationID == application.ApplicationID) && (tmpUser.UserID != (int)providerUserKey))
			{
				status = MembershipCreateStatus.DuplicateUserName;
				return false;
			}
		}

		User user1 = User.GetByID((int)providerUserKey);

		if (user1 == null)
		{
			status = MembershipCreateStatus.InvalidUserName;
			return false;
		}

		user1.FailedAnswerAttempts = 0;
		user1.FailedPasswordAttempts = 0;
		user1.LastActivity = DateTime.UtcNow;
		user1.LastLockout = null;
		user1.LastLogin = DateTime.UtcNow;
		user1.LastPasswordChange = DateTime.UtcNow;
		user1.Name = username;
		user1.Password = encodedPassword;
		user1.PasswordFormat = (int)PasswordFormat;
		if (Settings.SecurityQuestionRequired)
		{
			user1.PasswordAnswer = encodedPasswordAnswer;
			user1.PasswordQuestion = passwordQuestion;
		}
		user1.Salt = salt;
		user1.Save();

		status = MembershipCreateStatus.Success;

		return true;
	}

	public bool IsSecurityAnswerCorrect(string username, string answer)
	{
		string password;
		int status;
		string salt;
		int tempPasswordFormat;
		int failedPasswordAttemptCount;
		int failedPasswordAnswerAttemptCount;
		bool isApproved;
		DateTime lastLogin;
		DateTime lastActivity;

		if (EnablePasswordReset == false)
			throw new NotSupportedException("Not configured to support password resets");

		GetPasswordWithFormat(username, out status, out password, out tempPasswordFormat, out salt,
							  out failedPasswordAttemptCount, out failedPasswordAnswerAttemptCount, out isApproved,
							  out lastLogin, out lastActivity);
		if (status == 0)
		{
			if (answer != null)
				answer = answer.Trim();

			int securityAnswerPWFormat = tempPasswordFormat == (int)MembershipPasswordFormat.Hashed ? (int)MembershipPasswordFormat.Encrypted : (int)tempPasswordFormat;
			string encodedAnswer = !String.IsNullOrEmpty(answer)
									? EncodePassword(answer.ToLower(CultureInfo.InvariantCulture), securityAnswerPWFormat, salt)
									: answer;
			CheckParameter(ref encodedAnswer, RequiresQuestionAndAnswer, RequiresQuestionAndAnswer, false, 255, "password answer");

			List<User> users = User.UserGetByName(username);
			User user = null;
			foreach (User tmpUser in users)
			{
				if (tmpUser.ApplicationID == application.ApplicationID)
					user = tmpUser;
			}
			if (user == null)
				return false;
			if (!String.IsNullOrEmpty(user.PasswordAnswer) && !user.PasswordAnswer.Equals(encodedAnswer, StringComparison.OrdinalIgnoreCase))
			{
				user.FailedAnswerAttempts++;
				user.Save();
				return false;
			}
			user.LastActivity = DateTime.UtcNow;
			user.FailedAnswerAttempts = 0;
			user.FailedPasswordAttempts = 0;
			user.Save();

			return true;
		}
		return false;
	}

	#endregion

	#region implemented

	public override int MaxInvalidPasswordAttempts
	{
		get { return m_MaxInvalidPasswordAttempts; }
	}

	public override int MinRequiredNonAlphanumericCharacters
	{
		get { return m_MinRequiredNonalphanumericCharacters; }
	}

	public override int MinRequiredPasswordLength
	{
		get { return m_MinRequiredPasswordLength; }
	}

	public override int PasswordAttemptWindow
	{
		get { return m_PasswordAttemptWindow; }
	}

	public override MembershipPasswordFormat PasswordFormat
	{
		get { return m_PasswordFormat; }
	}

	public override string PasswordStrengthRegularExpression
	{
		get { return m_PasswordStrengthRegularExpression; }
	}

	public override bool RequiresQuestionAndAnswer
	{
		get { return m_RequiresQuestionAndAnswer; }
	}

	public override bool RequiresUniqueEmail
	{
		get { return m_RequiresUniqueEmail; }
	}

	#endregion
}