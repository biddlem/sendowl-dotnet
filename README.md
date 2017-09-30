# sendowl-dotnet
sendowl .net api client



## Usage

Initialize the client with your key and secret

```c#
var sendOwl = new SendOwlAPIClient("mykey", "mysecret");
```

Get product by id
```c#
 var product = await sendOwl.Product.GetAsync(123456);
```

Search for product by name
```c#
var products = await sendOwl.Products.SearchAsync("product name");
```

List all products
```c#
 var products = await sendOwl.Product.GetAllAsync();
```
