[![Build Status](https://travis-ci.com/challengerdeep/sendowl-dotnet.svg?token=npq1hDeEFws5qTkAZ6M4&branch=master)](https://travis-ci.com/challengerdeep/sendowl-dotnet)

# SendOwl-dotnet
SendOwl API client



## Usage

Initialize the client with your key and secret

```c#
SendOwlAPIClient sendOwl = new SendOwlAPIClient("mykey", "mysecret");
```

### Product

Get product by id
```c#
var product = await sendOwl.Product.GetAsync(123456);
```

Search for product by name
```c#
var products = await sendOwl.Product.SearchAsync("product name");
```

List all products
```c#
var products = await sendOwl.Product.GetAllAsync();
```

Create product
```c#
var product = await sendOwl.Product.CreateAsync(
  new SendOwlProduct
  {
    Name = "my product",
    Price = "19.99",
    Product_type = ProductType.Software
  });
```

Update product
```c#
var product = await sendOwl.Product.GetAsync(123456);
product.Price = "89.95";
product.Name = "new name";
await sendOwl.Product.UpdateAsync(product);
```

Delete product
```c#
await sendOwl.Product.DeleteAsync(256);
```

### Bundle

Get bundle by id
```c#
 var bundle = await sendOwl.Bundle.GetAsync(256);
```

List all bundles
```c#
 var bundles = await sendOwl.Bundle.GetAllAsync();
```

Create bundle
```c#
var bundle = await sendOwl.Bundle.CreateAsync(
  new SendOwlBundle
  {
    Name = "my new bundle",
    Price = "99.5",
    Components = new Components
    {
      Product_ids = new List<long>{ 256, 1024, 1337 }
    }
  });
```

Update Bundle
```c#
var bundle = await sendOwl.Bundle.GetAsync(256)
bundle.Price = "22.9";
bundle.Component.Product_Ids.Remove(1024);
await sendOwl.Bundle.UpdateAsync(bundle);
```

Delete bundle

```c#
 await sendOwl.Bundle.DeleteAsync(256);
```

