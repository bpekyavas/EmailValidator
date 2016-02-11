# EmailValidator

.Net Email Validator equivalent to apache EmailValidator



# EXAMPLE

´´´.NET

public static void Main()
{
	Console.WriteLine(EmailValidator.IsValid("onur.tuzun@[85.111.48.60]")); // True
	Console.WriteLine(EmailValidator.IsValid("onur.tuzun@iyzico.com")); // True
	Console.WriteLine(EmailValidator.IsValid("onur.tuzun@iyzico.com.tr")); // True
	Console.WriteLine(EmailValidator.IsValid("onur.123@iyzico.com")); // True
	Console.WriteLine(EmailValidator.IsValid("onur123.tuzun@iyzico.com")); // True
	Console.WriteLine(EmailValidator.IsValid("123@iyzico.com")); // True
	Console.WriteLine(EmailValidator.IsValid("*****@iyzico.com")); // True
	Console.WriteLine(EmailValidator.IsValid("_.-/*?^%&@iyzico.com")); // True
	Console.WriteLine("----");
	Console.WriteLine(EmailValidator.IsValid(null)); // False
	Console.WriteLine(EmailValidator.IsValid("")); // False
	Console.WriteLine(EmailValidator.IsValid("    ")); // False
	Console.WriteLine(EmailValidator.IsValid("@")); // False
	Console.WriteLine(EmailValidator.IsValid("@iyzico.com")); // False
	Console.WriteLine(EmailValidator.IsValid("onur.tuzun@")); // False
	Console.WriteLine(EmailValidator.IsValid("onur.tuzun@iyzico")); // False
	Console.WriteLine(EmailValidator.IsValid(".tuzun@iyzico.com")); // False
	Console.WriteLine(EmailValidator.IsValid("onur.tuzun@iyzico.com523")); // False
	Console.WriteLine(EmailValidator.IsValid("onur.tuzun@iyzico.11com")); // False
	Console.WriteLine(EmailValidator.IsValid("onur.tuzun@iyzico.con")); // False
	Console.WriteLine(EmailValidator.IsValid("onur.tuzun@iyzico.coom")); // False
	Console.WriteLine(EmailValidator.IsValid("onur.tuzun@iyzico.")); // False
}
	
´´´
