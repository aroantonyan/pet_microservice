// namespace ProductService.Tests.Helpers;
//
// public class ProductHelperTest
// {
//     [Theory]
//     [InlineData("Iphone", 1500, 1550)]
//     [InlineData("IPhone", 1500, 1560)]
//     [InlineData("IMac", 1500, 1560)]
//
//     public void AdjustPriceIfDesirable_ReturnsPricePlusFifty(string productName, double productPrice, double expectedPrice)
//     {
//         var result = ProductService.Helpers.ProductHelper.AdjustPriceIfDesirable(productName, productPrice);
//         Assert.Equal(expectedPrice, result);
//     }
// }