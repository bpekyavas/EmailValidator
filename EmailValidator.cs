using System;
using System.Net;
using System.Globalization;
using System.Text.RegularExpressions;

public class EmailValidator
{
	// Regex definitions
	private static String EMAIL_REGEX = "^\\s*?(.+)@(.+?)\\s*$";
	private static String SPECIAL_CHARS = "\\(\\)<>@,;:'\\\\\\\"\\.\\[\\]";
	private static String VALID_CHARS = "[^\\s" + SPECIAL_CHARS + "]";
	private static String QUOTED_USER = "(\"[^\"]*\")";
	private static String WORD = "((" + VALID_CHARS + "|')+|" + QUOTED_USER + ")";
	private static String USER_REGEX = "^\\s*" + WORD + "(\\." + WORD + ")*$";
	private static String IP_DOMAIN_REGEX = "^\\[(.*)\\]$";
	private static String DOMAIN_NAME_REGEX = "(?=^.{4,253}$)(^((?!-)[a-zA-Z0-9-]{1,63}(?<!-)\\.)+[a-zA-Z]{2,63}\\.?$)";
	
	// Domain and TLD definitions
	private static String[] INFRASTRUCTURE_TLDS = new String[]{"arpa"};	
	private static String[] GENERIC_TLDS = new String[]{"abogado", "academy", "accountants", "active", "actor", "adult", "aero", "agency", "airforce", "allfinanz", "alsace", "amsterdam", "android", "aquarelle", "archi", "army", "arpa", "asia", "associates", "attorney", "auction", "audio", "autos", "axa", "band", "bar", "bargains", "bayern", "beer", "berlin", "best", "bid", "bike", "bio", "biz", "black", "blackfriday", "bloomberg", "blue", "bmw", "bnpparibas", "boo", "boutique", "brussels", "budapest", "build", "builders", "business", "buzz", "bzh", "cab", "cal", "camera", "camp", "cancerresearch", "capetown", "capital", "caravan", "cards", "care", "career", "careers", "cartier", "casa", "cash", "cat", "catering", "center", "ceo", "cern", "channel", "cheap", "christmas", "chrome", "church", "citic", "city", "claims", "cleaning", "click", "clinic", "clothing", "club", "coach", "codes", "coffee", "college", "cologne", "com", "community", "company", "computer", "condos", "construction", "consulting", "contractors", "cooking", "cool", "coop", "country", "credit", "creditcard", "cricket", "crs", "cruises", "cuisinella", "cymru", "dad", "dance", "dating", "day", "deals", "degree", "delivery", "democrat", "dental", "dentist", "desi", "dev", "diamonds", "diet", "digital", "direct", "directory", "discount", "dnp", "docs", "domains", "doosan", "durban", "dvag", "eat", "edu", "education", "email", "emerck", "energy", "engineer", "engineering", "enterprises", "equipment", "esq", "estate", "eurovision", "eus", "events", "everbank", "exchange", "expert", "exposed", "fail", "farm", "fashion", "feedback", "finance", "financial", "firmdale", "fish", "fishing", "fitness", "flights", "florist", "flowers", "flsmidth", "fly", "foo", "forsale", "foundation", "frl", "frogans", "fund", "furniture", "futbol", "gal", "gallery", "garden", "gbiz", "gent", "ggee", "gift", "gifts", "gives", "glass", "gle", "global", "globo", "gmail", "gmo", "gmx", "google", "gop", "gov", "graphics", "gratis", "green", "gripe", "guide", "guitars", "guru", "hamburg", "haus", "healthcare", "help", "here", "hiphop", "hiv", "holdings", "holiday", "homes", "horse", "host", "hosting", "house", "how", "ibm", "immo", "immobilien", "industries", "info", "ing", "ink", "institute", "insure", "int", "international", "investments", "irish", "iwc", "jetzt", "jobs", "joburg", "juegos", "kaufen", "kim", "kitchen", "kiwi", "koeln", "krd", "kred", "lacaixa", "land", "latrobe", "lawyer", "lds", "lease", "legal", "lgbt", "lidl", "life", "lighting", "limited", "limo", "link", "loans", "london", "lotto", "ltda", "luxe", "luxury", "madrid", "maison", "management", "mango", "market", "marketing", "media", "meet", "melbourne", "meme", "memorial", "menu", "miami", "mil", "mini", "mobi", "moda", "moe", "monash", "money", "mormon", "mortgage", "moscow", "motorcycles", "mov", "museum", "nagoya", "name", "navy", "net", "network", "neustar", "new", "nexus", "ngo", "nhk", "ninja", "nra", "nrw", "nyc", "okinawa", "ong", "onl", "ooo", "org", "organic", "osaka", "otsuka", "ovh", "paris", "partners", "parts", "party", "pharmacy", "photo", "photography", "photos", "physio", "pics", "pictures", "pink", "pizza", "place", "plumbing", "pohl", "poker", "porn", "post", "praxi", "press", "pro", "prod", "productions", "prof", "properties", "property", "pub", "qpon", "quebec", "realtor", "recipes", "red", "rehab", "reise", "reisen", "reit", "ren", "rentals", "repair", "report", "republican", "rest", "restaurant", "reviews", "rich", "rio", "rip", "rocks", "rodeo", "rsvp", "ruhr", "ryukyu", "saarland", "sale", "samsung", "sarl", "sca", "scb", "schmidt", "schule", "schwarz", "science", "scot", "services", "sew", "sexy", "shiksha", "shoes", "shriram", "singles", "sky", "social", "software", "sohu", "solar", "solutions", "soy", "space", "spiegel", "supplies", "supply", "support", "surf", "surgery", "suzuki", "sydney", "systems", "taipei", "tatar", "tattoo", "tax", "technology", "tel", "tienda", "tips", "tires", "tirol", "today", "tokyo", "tools", "top", "town", "toys", "trade", "training", "travel", "trust", "tui", "university", "uno", "uol", "vacations", "vegas", "ventures", "versicherung", "vet", "viajes", "video", "villas", "vision", "vlaanderen", "vodka", "vote", "voting", "voto", "voyage", "wales", "wang", "watch", "webcam", "website", "wed", "wedding", "whoswho", "wien", "wiki", "williamhill", "wme", "work", "works", "world", "wtc", "wtf", "xn--1qqw23a", "xn--3bst00m", "xn--3ds443g", "xn--45q11c", "xn--4gbrim", "xn--55qw42g", "xn--55qx5d", "xn--6frz82g", "xn--6qq986b3xl", "xn--80adxhks", "xn--80asehdb", "xn--80aswg", "xn--c1avg", "xn--cg4bki", "xn--czr694b", "xn--czrs0t", "xn--czru2d", "xn--d1acj3b", "xn--fiq228c5hs", "xn--fiq64b", "xn--flw351e", "xn--hxt814e", "xn--i1b6b1a6a2e", "xn--io0a7i", "xn--kput3i", "xn--mgbab2bd", "xn--ngbc5azd", "xn--nqv7f", "xn--nqv7fs00ema", "xn--p1acf", "xn--q9jyb4c", "xn--qcka1pmc", "xn--rhqv96g", "xn--ses554g", "xn--unup4y", "xn--vermgensberater-ctb", "xn--vermgensberatung-pwb", "xn--vhquv", "xn--xhq521b", "xn--zfr164b", "xxx", "xyz", "yachts", "yandex", "yoga", "yokohama", "youtube", "zip", "zone", "zuerich"};
	private static String[] COUNTRY_CODE_TLDS = new String[]{"ac", "ad", "ae", "af", "ag", "ai", "al", "am", "an", "ao", "aq", "ar", "as", "at", "au", "aw", "ax", "az", "ba", "bb", "bd", "be", "bf", "bg", "bh", "bi", "bj", "bm", "bn", "bo", "br", "bs", "bt", "bv", "bw", "by", "bz", "ca", "cc", "cd", "cf", "cg", "ch", "ci", "ck", "cl", "cm", "cn", "co", "cr", "cu", "cv", "cw", "cx", "cy", "cz", "de", "dj", "dk", "dm", "do", "dz", "ec", "ee", "eg", "er", "es", "et", "eu", "fi", "fj", "fk", "fm", "fo", "fr", "ga", "gb", "gd", "ge", "gf", "gg", "gh", "gi", "gl", "gm", "gn", "gp", "gq", "gr", "gs", "gt", "gu", "gw", "gy", "hk", "hm", "hn", "hr", "ht", "hu", "id", "ie", "il", "im", "in", "io", "iq", "ir", "is", "it", "je", "jm", "jo", "jp", "ke", "kg", "kh", "ki", "km", "kn", "kp", "kr", "kw", "ky", "kz", "la", "lb", "lc", "li", "lk", "lr", "ls", "lt", "lu", "lv", "ly", "ma", "mc", "md", "me", "mg", "mh", "mk", "ml", "mm", "mn", "mo", "mp", "mq", "mr", "ms", "mt", "mu", "mv", "mw", "mx", "my", "mz", "na", "nc", "ne", "nf", "ng", "ni", "nl", "no", "np", "nr", "nu", "nz", "om", "pa", "pe", "pf", "pg", "ph", "pk", "pl", "pm", "pn", "pr", "ps", "pt", "pw", "py", "qa", "re", "ro", "rs", "ru", "rw", "sa", "sb", "sc", "sd", "se", "sg", "sh", "si", "sj", "sk", "sl", "sm", "sn", "so", "sr", "st", "su", "sv", "sx", "sy", "sz", "tc", "td", "tf", "tg", "th", "tj", "tk", "tl", "tm", "tn", "to", "tp", "tr", "tt", "tv", "tw", "tz", "ua", "ug", "uk", "us", "uy", "uz", "va", "vc", "ve", "vg", "vi", "vn", "vu", "wf", "ws", "xn--3e0b707e", "xn--45brj9c", "xn--80ao21a", "xn--90a3ac", "xn--clchc0ea0b2g2a9gcd", "xn--d1alf", "xn--fiqs8s", "xn--fiqz9s", "xn--fpcrj9c3d", "xn--fzc2c9e2c", "xn--gecrj9c", "xn--h2brj9c", "xn--j1amh", "xn--j6w193g", "xn--kprw13d", "xn--kpry57d", "xn--l1acc", "xn--lgbbat1ad8j", "xn--mgb9awbf", "xn--mgba3a4f16a", "xn--mgbaam7a8h", "xn--mgbayh7gpa", "xn--mgbbh1a71e", "xn--mgbc0a9azcg", "xn--mgberp4a5d4ar", "xn--mgbx4cd0ab", "xn--node", "xn--o3cw4h", "xn--ogbpf8fl", "xn--p1ai", "xn--pgbs0dh", "xn--s9brj9c", "xn--wgbh1c", "xn--wgbl6a", "xn--xkc2al3hye2a", "xn--xkc2dl3a5ee0h", "xn--yfro4i67o", "xn--ygbi2ammx", "ye", "yt", "za", "zm", "zw"};
	private static String[] LOCAL_TLDS = new String[]{"localdomain", "localhost"};
	
	public static bool IsValid(string email)
	{
		if (email == null)
		{
			return false;
		}

		if (email.EndsWith("."))
		{
			return false;
		}

		Regex r = new Regex(EMAIL_REGEX, RegexOptions.IgnoreCase);
		Match m = r.Match(email);
		if (!m.Success)
		{
			return false;
		}

		if (!IsValidUser(m.Groups[1].ToString()))
		{
			return false;
		}

		if (!IsValidDomain(m.Groups[2].ToString()))
		{
			return false;
		}

		return true;
	}

	public static bool IsValidUser(String user)
	{
		Regex r = new Regex(USER_REGEX, RegexOptions.IgnoreCase);
		Match m = r.Match(user);
		return m.Success ? true : false;
	}

	public static bool IsValidDomain(String domain)
	{
		Regex r = new Regex(IP_DOMAIN_REGEX, RegexOptions.IgnoreCase);
		Match m = r.Match(domain);
		if (m.Success)
		{
			IPAddress ipAddress;
			return IPAddress.TryParse(m.Groups[1].ToString(), out ipAddress);
		}

		Boolean mappingInvalid = false;
		try
		{
			domain = new IdnMapping().GetAscii(domain);
		}
		catch (ArgumentException)
		{
			mappingInvalid = true;
		}

		if (mappingInvalid)
		{
			return false;
		}

		if (Regex.IsMatch(domain, DOMAIN_NAME_REGEX, RegexOptions.IgnoreCase))
		{
			String[] group = domain.Split('.');
			if (group.Length >2)
			{
				return IsValidTld(group[2]);
			}
			return IsValidTld(group[1]);
		}
		return false;
	}

	public static bool IsValidTld(String tld)
	{
		return IsValidLocalTld(tld) ? true : IsValidInfrastructureTld(tld) || IsValidGenericTld(tld) || IsValidCountryCodeTld(tld);
	}

	public static bool IsValidInfrastructureTld(String iTld)
	{
		return Array.BinarySearch(INFRASTRUCTURE_TLDS, ChompLeadingDot(iTld.ToLower())) >= 0;
	}

	public static bool IsValidGenericTld(String gTld)
	{
		return Array.BinarySearch(GENERIC_TLDS, ChompLeadingDot(gTld.ToLower())) >= 0;
	}

	public static bool IsValidCountryCodeTld(String ccTld)
	{
		return Array.BinarySearch(COUNTRY_CODE_TLDS, ChompLeadingDot(ccTld.ToLower())) >= 0;
	}

	public static bool IsValidLocalTld(String lTld)
	{
		return Array.BinarySearch(LOCAL_TLDS, ChompLeadingDot(lTld.ToLower())) >= 0;
	}

	public static String ChompLeadingDot(String str)
	{
		return str.StartsWith(".") ? str.Substring(1) : str;
	}
}
