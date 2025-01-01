using UGHApi.Models;

namespace UGHApi.Services.HtmlTemplate;

public class HtmlTemplateService
{
    private readonly IConfiguration _configuration;
    private readonly string _baseUrl;

    public HtmlTemplateService(IConfiguration configuration)
    {
        _configuration = configuration;
        _baseUrl = _configuration.GetValue<string>("FrontendBaseUrl");
    }

    public string GetEmailVerifiedHtml()
    {
        return $@"
        <!DOCTYPE html>
        <html lang='en'>
        <head>
            <meta charset='UTF-8'>
            <meta name='viewport' content='width=device-width, initial-scale=1.0'>
            <title>Email Verified</title>
            <style>
                body {{
                    font-family: Arial, sans-serif;
                    background-color: #f9f9f9;
                    margin: 0;
                    padding: 0;
                    display: flex;
                    justify-content: center;
                    align-items: center;
                    height: 100vh;
                    color: #333;
                }}
                .container {{
                    text-align: center;
                    background: #fff;
                    padding: 20px 30px;
                    border-radius: 10px;
                    box-shadow: 0 4px 6px rgba(0, 0, 0, 0.1);
                }}
                .success-icon {{
                    font-size: 50px;
                    color: #4CAF50;
                    margin-bottom: 10px;
                }}
                h1 {{
                    margin: 0;
                    font-size: 24px;
                    color: #4CAF50;
                }}
                p {{
                    font-size: 16px;
                    color: #666;
                }}
                a {{
                    display: inline-block;
                    margin-top: 15px;
                    text-decoration: none;
                    color: #fff;
                    background-color: #4CAF50;
                    padding: 10px 20px;
                    border-radius: 5px;
                    font-size: 14px;
                    font-weight: bold;
                }}
                a:hover {{
                    background-color: #45a049;
                }}
            </style>
        </head>
        <body>
            <div class='container'>
                <div class='success-icon'>👍</div>
                <h1>Email Verified Successfully</h1>
                <p>Congratulations! Your email has been successfully verified.</p>
                <a href='{_baseUrl}/login'>Go to Login Page</a>
            </div>
        </body>
        </html>";
    }

    public string GetInvalidTokenHtml()
    {
        return $@"
                <!DOCTYPE html>
                <html lang='en'>
                <head>
                    <meta charset='UTF-8'>
                    <meta name='viewport' content='width=device-width, initial-scale=1.0'>
                    <title>Invalid Token</title>
                    <style>
                        body {{
                            font-family: Arial, sans-serif;
                            background-color: #f9f9f9;
                            margin: 0;
                            padding: 0;
                            display: flex;
                            justify-content: center;
                            align-items: center;
                            height: 100vh;
                            color: #333;
                        }}
                        .container {{
                            text-align: center;
                            background: #fff;
                            padding: 20px 30px;
                            border-radius: 10px;
                            box-shadow: 0 4px 6px rgba(0, 0, 0, 0.1);
                        }}
                        .error-icon {{
                            font-size: 50px;
                            color: #e53935;
                            margin-bottom: 10px;
                        }}
                        h1 {{
                            margin: 0;
                            font-size: 24px;
                            color: #e53935;
                        }}
                        p {{
                            font-size: 16px;
                            color: #666;
                        }}
                        a {{
                            display: inline-block;
                            margin-top: 15px;
                            text-decoration: none;
                            color: #fff;
                            background-color: #e53935;
                            padding: 10px 20px;
                            border-radius: 5px;
                            font-size: 14px;
                            font-weight: bold;
                        }}
                        a:hover {{
                            background-color: #d32f2f;
                        }}
                    </style>
                </head>
                <body>
                    <div class='container'>
                        <div class='error-icon'>❌</div>
                        <h1>Invalid Token</h1>
                        <p>Sorry, the token provided is not valid. Please check the link or try again.</p>
                        <a href='{_baseUrl}/verify-email'>Request New Token</a>
                    </div>
                </body>
                </html>";
    }

    public CouponRecievedTemplate GetCouponReceivedDetails(string couponCode, string userFullName)
    {
        string subject = $"Congratulations, {userFullName}! Your Membership Coupon is Here!";
        var htmlContent =
            $@"
    <!DOCTYPE html>
    <html lang='en'>
    <head>
        <meta charset='UTF-8'>
        <meta name='viewport' content='width=device-width, initial-scale=1.0'>
        <title>Membership Coupon Received</title>
        <style>
            body {{
                font-family: 'Arial', sans-serif;
                background: linear-gradient(135deg, #ff9a9e, #fad0c4);
                margin: 0;
                padding: 0;
                display: flex;
                justify-content: center;
                align-items: center;
                height: 100vh;
                color: #001f3f;
            }}
            .container {{
                text-align: center;
                background: rgba(255, 255, 255, 0.9);
                padding: 25px 40px;
                border-radius: 15px;
                box-shadow: 0 8px 20px rgba(0, 0, 0, 0.3);
            }}
            .icon {{
                font-size: 60px;
                color: #ffc107;
                margin-bottom: 15px;
            }}
            h1 {{
                margin: 0;
                font-size: 32px;
                color: #ffc107;
            }}
            p {{
                font-size: 16px;
                color: #333333;
                margin: 10px 0 20px;
            }}
            .highlight {{
                color: #001f3f;
                font-weight: bold;
            }}
            .coupon {{
                display: inline-block;
                background: #ffc107;
                color: #001f3f;
                padding: 10px 25px;
                border-radius: 5px;
                font-weight: bold;
                font-size: 20px;
                text-shadow: 0 1px 3px rgba(0, 0, 0, 0.3);
                margin: 15px 0;
            }}
            a {{
                display: inline-block;
                text-decoration: none;
                color: #fff;
                background-color: #001f3f;
                padding: 10px 20px;
                border-radius: 5px;
                font-size: 16px;
                font-weight: bold;
                box-shadow: 0 6px 10px rgba(0, 0, 0, 0.2);
                transition: background-color 0.3s;
            }}
            a:hover {{
                background-color: #004080;
            }}
            .footer {{
                margin-top: 20px;
                font-size: 12px;
                color: #666;
            }}
        </style>
    </head>
    <body>
        <div class='container'>
            <div class='icon'>🎉</div>
            <h1>Congratulations, {userFullName}!</h1>
            <p>You are now eligible for our exclusive membership benefits.</p>
            <p>Your membership coupon code is:</p>
            <div class='coupon'>{couponCode}</div>
            <p class='highlight'>What's Next?</p>
            <p>Use your coupon to unlock premium features, gain access to special offers, and enjoy exclusive perks designed just for our members.</p>
            <a href='{_baseUrl}/home'>Redeem Your Coupon</a>
        </div>
    </body>
    </html>";

        return new CouponRecievedTemplate() { Subject = subject, BodyHtml = htmlContent };
    }
}
