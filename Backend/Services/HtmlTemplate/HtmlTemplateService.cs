namespace UGHApi.Services.HtmlTemplate;

public class HtmlTemplateService
{
    public string GetEmailVerifiedHtml()
    {
        return @"
        <!DOCTYPE html>
        <html lang='en'>
        <head>
            <meta charset='UTF-8'>
            <meta name='viewport' content='width=device-width, initial-scale=1.0'>
            <title>Email Verified</title>
            <style>
                body {
                    font-family: Arial, sans-serif;
                    background-color: #f9f9f9;
                    margin: 0;
                    padding: 0;
                    display: flex;
                    justify-content: center;
                    align-items: center;
                    height: 100vh;
                    color: #333;
                }
                .container {
                    text-align: center;
                    background: #fff;
                    padding: 20px 30px;
                    border-radius: 10px;
                    box-shadow: 0 4px 6px rgba(0, 0, 0, 0.1);
                }
                .success-icon {
                    font-size: 50px;
                    color: #4CAF50;
                    margin-bottom: 10px;
                }
                h1 {
                    margin: 0;
                    font-size: 24px;
                    color: #4CAF50;
                }
                p {
                    font-size: 16px;
                    color: #666;
                }
                a {
                    display: inline-block;
                    margin-top: 15px;
                    text-decoration: none;
                    color: #fff;
                    background-color: #4CAF50;
                    padding: 10px 20px;
                    border-radius: 5px;
                    font-size: 14px;
                    font-weight: bold;
                }
                a:hover {
                    background-color: #45a049;
                }
            </style>
        </head>
        <body>
            <div class='container'>
                <div class='success-icon'>👍</div>
                <h1>Email Verified Successfully</h1>
                <p>Congratulations! Your email has been successfully verified.</p>
                <a href='http://ugh.csdevhub.com:3000/login'>Go to Login Page</a>
            </div>
        </body>
        </html>";
    }

    public string GetInvalidTokenHtml()
    {
        return @"
                <!DOCTYPE html>
                <html lang='en'>
                <head>
                    <meta charset='UTF-8'>
                    <meta name='viewport' content='width=device-width, initial-scale=1.0'>
                    <title>Invalid Token</title>
                    <style>
                        body {
                            font-family: Arial, sans-serif;
                            background-color: #f9f9f9;
                            margin: 0;
                            padding: 0;
                            display: flex;
                            justify-content: center;
                            align-items: center;
                            height: 100vh;
                            color: #333;
                        }
                        .container {
                            text-align: center;
                            background: #fff;
                            padding: 20px 30px;
                            border-radius: 10px;
                            box-shadow: 0 4px 6px rgba(0, 0, 0, 0.1);
                        }
                        .error-icon {
                            font-size: 50px;
                            color: #e53935;
                            margin-bottom: 10px;
                        }
                        h1 {
                            margin: 0;
                            font-size: 24px;
                            color: #e53935;
                        }
                        p {
                            font-size: 16px;
                            color: #666;
                        }
                        a {
                            display: inline-block;
                            margin-top: 15px;
                            text-decoration: none;
                            color: #fff;
                            background-color: #e53935;
                            padding: 10px 20px;
                            border-radius: 5px;
                            font-size: 14px;
                            font-weight: bold;
                        }
                        a:hover {
                            background-color: #d32f2f;
                        }
                    </style>
                </head>
                <body>
                    <div class='container'>
                        <div class='error-icon'>❌</div>
                        <h1>Invalid Token</h1>
                        <p>Sorry, the token provided is not valid. Please check the link or try again.</p>
                        <a href='http://ugh.csdevhub.com:3000/verify-email'>Request New Token</a>
                    </div>
                </body>
                </html>";
    }
}
