using Core.Entities;
using Core.Interfaces;
using Microsoft.Extensions.Configuration;
using Stripe;

namespace Infrastructure.Services;

public class CouponService : ICouponService
{
    public CouponService(IConfiguration config)
    {
        StripeConfiguration.ApiKey = config["StripeSettings:SecretKey"];
    }

    public async Task<AppCoupon?> GetCouponFromPromoCode(string code)
    {
        var promotionService = new PromotionCodeService();
        var couponService = new Stripe.CouponService();
        var options = new PromotionCodeListOptions
        {
            Code = code
        };
        var promotionCodes = await promotionService.ListAsync(options);
        var promotionCode = promotionCodes.FirstOrDefault();

        var coupon = await couponService.GetAsync(promotionCode.Promotion.CouponId);

        if (promotionCode != null && coupon != null)
        {
            return new AppCoupon
            {
                Name = coupon.Name,
                AmountOff = coupon.AmountOff,
                PercentOff = coupon.PercentOff,
                CouponId = coupon.Id,
                PromotionCode = promotionCode.Code
            };
        }
        return null;
    }
}
