using UGHApi.Models;

namespace UGHApi.Services.HtmlTemplate;

public class HtmlTemplateService
{
    private readonly IConfiguration _configuration;
    private readonly string _baseUrl;

    public HtmlTemplateService(IConfiguration configuration)
    {
        _configuration = configuration;
        _baseUrl = _configuration.GetValue<string>("BaseUrlFrontend");
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
                <div class='success-icon'>??</div>
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
                        <div class='error-icon'>?</div>
                        <h1>Invalid Token</h1>
                        <p>Sorry, the token provided is not valid. Please check the link or try again.</p>
                        <a href='{_baseUrl}/verify-email'>Request New Token</a>
                    </div>
                </body>
                </html>";
    }

    public CouponRecievedTemplate GetCouponReceivedDetails(string couponCode, string userFullName, string membershipName, int durationMonths)
    {
        string subject = $"Glückwunsch, {userFullName}! Dein Mitgliedschafts-Coupon ist da!";
        var htmlContent =
            $@"
    <!DOCTYPE html>
    <html lang='de'>
    <head>
        <meta charset='UTF-8'>
        <meta name='viewport' content='width=device-width, initial-scale=1.0'>
        <title>Mitgliedschafts-Coupon erhalten</title>
        <style>
            :root {{
                --primary-yellow: #ffc107;
                --primary-blue: #001f3f;
                --secondary-blue: #004080;
                --text-dark: #333333;
                --text-light: #666666;
                --white: #ffffff;
                --shadow: rgba(0, 0, 0, 0.3);
            }}
            body {{
                font-family: 'Arial', sans-serif;
                background: linear-gradient(135deg, #ff9a9e, #fad0c4);
                margin: 0;
                padding: 0;
                display: flex;
                justify-content: center;
                align-items: center;
                min-height: 100vh;
                color: var(--primary-blue);
            }}
            .container {{
                text-align: center;
                background: rgba(255, 255, 255, 0.95);
                padding: 30px 40px;
                border-radius: 20px;
                box-shadow: 0 10px 30px var(--shadow);
                max-width: 500px;
                margin: 20px;
            }}
            h1 {{
                margin: 0 0 15px 0;
                font-size: 28px;
                color: var(--primary-yellow);
                text-shadow: 1px 1px 2px rgba(0, 0, 0, 0.1);
            }}
            p {{
                font-size: 16px;
                color: var(--text-dark);
                margin: 10px 0 15px;
                line-height: 1.5;
            }}
            .highlight {{
                color: var(--primary-blue);
                font-weight: bold;
                font-size: 18px;
                margin: 20px 0 10px;
            }}
            .coupon {{
                display: flex;
                flex-direction: column;
                align-items: center;
                gap: 8px;
                margin: 20px 0;
            }}
            .coupon-label {{
                font-weight: bold;
                color: var(--primary-blue);
                margin-bottom: 4px;
            }}
            .coupon-input {{
                font-size: 20px;
                font-weight: bold;
                padding: 8px 16px;
                border-radius: 8px;
                border: 1px solid var(--primary-yellow);
                background: var(--white);
                color: var(--primary-blue);
                text-align: center;
                width: 90%;
                max-width: 320px;
                letter-spacing: 2px;
            }}
            .copy-hint {{
                font-size: 13px;
                color: var(--text-light);
            }}
            a {{
                display: inline-block;
                text-decoration: none;
                color: var(--white);
                background-color: var(--primary-blue);
                padding: 12px 25px;
                border-radius: 8px;
                font-size: 16px;
                font-weight: bold;
                box-shadow: 0 6px 15px rgba(0, 0, 0, 0.2);
                transition: all 0.3s ease;
                margin-top: 15px;
            }}
            a:hover {{
                background-color: var(--secondary-blue);
                transform: translateY(-2px);
                box-shadow: 0 8px 20px rgba(0, 0, 0, 0.3);
            }}
            .footer {{
                margin-top: 25px;
                font-size: 12px;
                color: var(--text-light);
                border-top: 1px solid #eee;
                padding-top: 15px;
            }}
        </style>
    </head>
    <body>
        <div class='container'>
            <h1>Glückwunsch, {userFullName}!</h1>
            <p>Du bist jetzt berechtigt für unsere exklusiven Mitgliedschaftsvorteile.</p>
            <p><b>Mitgliedschaft:</b> {membershipName} ({durationMonths} Monate)</p>
            <div class='coupon'>
                <span class='coupon-label'>Dein Coupon-Code:</span>
                <input class='coupon-input' value='{couponCode}' readonly>
                <span class='copy-hint'>Zum Kopieren markieren und STRG+C drücken.</span>
            </div>
            <p class='highlight'>Was kommt als nächstes?</p>
            <p>Nutze deinen Coupon, um Premium-Features freizuschalten, Zugang zur Plattform und exklusive Vorteile zu genießen, die speziell für unsere Mitglieder entwickelt wurden.</p>
            <a href='{_baseUrl}/home'>Coupon einlösen</a>
            <div class='footer'>
                <p>Vielen Dank für dein Vertrauen in unsere Urlaub gegen Hand Community!</p>
            </div>
        </div>
    </body>
    </html>";

        return new CouponRecievedTemplate() { Subject = subject, BodyHtml = htmlContent };
    }

    public CouponRecievedTemplate GetCouponPurchasedDetails(string couponCode, string userFullName, string membershipName, int durationMonths)
    {
        string subject = $"Vielen Dank, {userFullName}! Dein Mitgliedschafts-Coupon wurde gekauft!";
        var htmlContent =
            $@"
<!DOCTYPE html>
<html lang='de'>
<head>
    <meta charset='UTF-8'>
    <meta name='viewport' content='width=device-width, initial-scale=1.0'>
    <title>Mitgliedschafts-Coupon gekauft</title>
    <style>
        :root {{
            --primary-yellow: #ffc107;
            --primary-blue: #001f3f;
            --secondary-blue: #004080;
            --text-dark: #333333;
            --text-light: #666666;
            --white: #ffffff;
            --shadow: rgba(0, 0, 0, 0.3);
        }}
        body {{
            font-family: 'Arial', sans-serif;
            background: linear-gradient(135deg, #ff9a9e, #fad0c4);
            margin: 0;
            padding: 0;
            display: flex;
            justify-content: center;
            align-items: center;
            min-height: 100vh;
            color: var(--primary-blue);
        }}
        .container {{
            text-align: center;
            background: rgba(255, 255, 255, 0.95);
            padding: 30px 40px;
            border-radius: 20px;
            box-shadow: 0 10px 30px var(--shadow);
            max-width: 500px;
            margin: 20px;
        }}
        h1 {{
            margin: 0 0 15px 0;
            font-size: 28px;
            color: var(--primary-yellow);
            text-shadow: 1px 1px 2px rgba(0, 0, 0, 0.1);
        }}
        p {{
            font-size: 16px;
            color: var(--text-dark);
            margin: 10px 0 15px;
            line-height: 1.5;
        }}
        .highlight {{
            color: var(--primary-blue);
            font-weight: bold;
            font-size: 18px;
            margin: 20px 0 10px;
        }}
        .coupon {{
            display: flex;
            flex-direction: column;
            align-items: center;
            gap: 8px;
            margin: 20px 0;
        }}
        .coupon-label {{
            font-weight: bold;
            color: var(--primary-blue);
            margin-bottom: 4px;
        }}
        .coupon-input {{
            font-size: 20px;
            font-weight: bold;
            padding: 8px 16px;
            border-radius: 8px;
            border: 1px solid var(--primary-yellow);
            background: var(--white);
            color: var(--primary-blue);
            text-align: center;
            width: 90%;
            max-width: 320px;
            letter-spacing: 2px;
        }}
        .copy-hint {{
            font-size: 13px;
            color: var(--text-light);
        }}
        a {{
            display: inline-block;
            text-decoration: none;
            color: var(--white);
            background-color: var(--primary-blue);
            padding: 12px 25px;
            border-radius: 8px;
            font-size: 16px;
            font-weight: bold;
            box-shadow: 0 6px 15px rgba(0, 0, 0, 0.2);
            transition: all 0.3s ease;
            margin-top: 15px;
        }}
        a:hover {{
            background-color: var(--secondary-blue);
            transform: translateY(-2px);
            box-shadow: 0 8px 20px rgba(0, 0, 0, 0.3);
        }}
        .footer {{
            margin-top: 25px;
            font-size: 12px;
            color: var(--text-light);
            border-top: 1px solid #eee;
            padding-top: 15px;
        }}
    </style>
</head>
<body>
    <div class='container'>
        <h1>Vielen Dank, {userFullName}!</h1>
        <p>Dein Kauf war erfolgreich! Du bist jetzt berechtigt für unsere exklusiven Mitgliedschaftsvorteile.</p>
        <p><b>Mitgliedschaft:</b> {membershipName} ({durationMonths} Monate)</p>
        <div class='coupon'>
            <span class='coupon-label'>Dein Coupon-Code:</span>
            <input class='coupon-input' value='{couponCode}' readonly>
            <span class='copy-hint'>Zum Kopieren markieren und STRG+C drücken.</span>
        </div>
        <p class='highlight'>Wie kannst du ihn einlösen?</p>
        <p>Klicke auf den Button unten, um deinen Coupon einzulösen und Premium-Features freizuschalten, Zugang zu besonderen Angeboten zu erhalten und exklusive Vorteile zu genießen, die speziell für unsere Mitglieder entwickelt wurden.</p>
        <a href='{_baseUrl}/home'>Coupon einlösen</a>
        <div class='footer'>
            <p>Vielen Dank für dein Vertrauen in unsere Community!</p>
        </div>
    </div>
</body>
</html>";

        return new CouponRecievedTemplate() { Subject = subject, BodyHtml = htmlContent };
    }
}
