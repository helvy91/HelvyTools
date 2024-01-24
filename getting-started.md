1. Attributes:

Most of the time all metadata you need is some string value for class, property etc. That's why I've created NameAttribute which can be inherited if more data is needed. Reading functions are based on this attribute.

```
var className = AttributeReader.ReadClassName<NameAttribute, Test>();
Console.WriteLine(className);

var propNames = AttributeReader.ReadPropertyNames<NameAttribute, Test>();
foreach (var item in propNames)
{
    Console.WriteLine(item.Key);
}

var enumName = AttributeReader.ReadEnumName<NameAttribute, TestEnum>(TestEnum.Member1);
Console.WriteLine(enumName);
```

2. Configuration:

If you need a place to put your configuration data and a way to retrieve it from various sources, you can use Configuration class. Just create properties which will store important config data and choose a provider/source (for now  AppSettings, AzureSettings or MysqlDB).

```
public class SmtpConfiguration : Configuration
{
    public SmtpConfiguration(ConfigProvider provider) : base(provider) { }

    public string Username
        => GetOrFail<string>("AppSettings:Smtp:Username");
    public string Password
        => GetOrFail<string>("AppSettings:Smtp:Password");
    public string FromAddress
        => GetOrFail<string>("AppSettings:Smtp:FromAddress");
    public string Host
        => GetOrFail<string>("AppSettings:Smtp:Host");
    public int Port
        => GetOrFail<int>("AppSettings:Smtp:Port");
}
```
3. Http requests:

HtpClient is quite simple class, but still requires some redundand code, especially when it comes to sending requests with content. Also Get() method doesn't take into consideration content in response. I fixed those isues with HelvyTools.Http.HttpClient. 

```
HttpClientHandler handler = new HttpClientHandler()
{
    Proxy = new WebProxy("http://127.0.0.1:8888"),
    UseProxy = true,
};
var netHttpClient = new System.Net.Http.HttpClient(handler);
var httpClient = new HelvyTools.Http.HttpClient(netHttpClient);

var response = httpClient.GetAsync<string>("https://test.pl");
```

4. SQL connection:

Connecting to SQL without Entity or other ORM is quite complex thing in .NET. With HelvyTools.MySqlConnector it's much easier. If name of property is different than column in table, use MySqlPropertyName as shown below. Then just create MySqlConnector instance and use ExecuteCommand() function.

```
public class TestSqlClass
{
    [MySqlPropertyName("id")]
    public int Id { get; set; }

    [MySqlPropertyName("customer_name")]
    public string Name { get; set; }

    [MySqlPropertyName("customer_surname")]
    public string Surname { get; set; }
}
```

```
var sqlConnector = new HelvyTools.Sql.MySqlConnector("connString");

sqlConnector.ExecuteCommandAsync("UPDATE test SET value = 1 WHERE key = 'test'");
var testResult = sqlConnector.ExecuteCommandAsync<Test>("SELECT id, name,surname FROM testDb");
```

5. Serialization

Serializing in .NET requires at least several lines of code. If we use this code over and over in project we have a lot of additional code we have to maintain, plus writing the same code repeatedly requires more time.

```
var test = new Test() { Property1 = 1 };

var xmlSerialized = XmlUtils.Serialize<Test>(test);
var xmlDeserialized = XmlUtils.Deserialize<Test>(xmlSerialized);

var jsonSerialized = JsonUtils.Serialize(test);
var jsonDeserialized = JsonUtils.Deserialize<Test>(jsonSerialized);
```

6. HTML parsing

When parsing HTML I always used HtmlAgilityPack. Unfortunately most of the time the interface was too complex and required some redundant code. With HelvyTools.HtmlParser parsing HTML is easy and straightforward. Most needed functions with simple  interface in one class.

```
var html = @"<html>
                <body>
                    <h1>TEST TITLE</h1>
                </body>
              </html>";
var parser = new HtmlParser();
var tag = parser.GetElementByTagName("h1", html, 3);
Console.WriteLine(tag.InnerText); 
```
