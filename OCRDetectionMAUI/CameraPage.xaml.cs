using BarcodeQrScanner.Services;
using SkiaSharp;
using SkiaSharp.Views.Maui;
using Plugin.Maui.OCR;

namespace BarcodeQrScanner
{
    public partial class CameraPage : ContentPage
    {
        private int imageWidth;
        private int imageHeight;
        OcrOptions options;
        public CameraPage()
        {
            InitializeComponent();
        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();

            await OcrPlugin.Default.InitAsync();
            var ohipPattern = new OcrPatternConfig(@"\d{4} \d{3} \d{3}", IsValidLuhn);
            var patternConfigsList = new List<OcrPatternConfig> { ohipPattern };

            options = new OcrOptions.Builder().SetTryHard(true).SetPatternConfigs(patternConfigsList).Build();


        }

        private void CameraPreview_ResultReady(object sender, ResultReadyEventArgs e)
        {
            Device.BeginInvokeOnMainThread(async () =>
            {
            if (e.Result != null)
                {
                    try
                    {

                        var data = (byte[])e.Result;
                        var ocrResult = await OcrPlugin.Default.RecognizeTextAsync(data, options);

                        if (!ocrResult.Success)
                        {
                            return;
                        }
                        else
                        {
                            var patientHcn = ocrResult.MatchedValues.FirstOrDefault(); // This will be the HCN (and only the HCN) if it's found
                        
                            if (patientHcn != null)
                            {
                            
                                // Provide feedback on the UI
                                ResultLabel.Text = $"OCR Result: {patientHcn}";
                                // Show a message or update UI to indicate the number is found
                                //Toast.MakeText(Application.Context, "Health Card Number Found!", ToastLength.Long).Show();
                           
                            }
                        }
                    }
                    catch(Exception ex)
                    { }

                }
                else
                {
                    ResultLabel.Text = "";
                }

            });
            imageWidth = e.PreviewWidth;
            imageHeight = e.PreviewHeight;

            canvasView.InvalidateSurface();
        }

        private bool IsValidLuhn(string number)
        {
            // Remove all non-digit characters (spaces in this case)
            number = new string(number.Where(char.IsDigit).ToArray());

            // Convert the string to an array of digits
            int[] digits = number.Select(d => int.Parse(d.ToString())).ToArray();
            int checkDigit = 0;

            // Luhn algorithm implementation
            for (int i = digits.Length - 2; i >= 0; i--)
            {
                int currentDigit = digits[i];
                if ((digits.Length - 2 - i) % 2 == 0) // check if it's an even index from the right
                {
                    currentDigit *= 2;
                    if (currentDigit > 9)
                    {
                        currentDigit -= 9;
                    }
                }
                checkDigit += currentDigit;
            }

            return (10 - (checkDigit % 10)) % 10 == digits.Last();
        }
    

    void DisplayScannedData()
        {
            //if (data != null)
            //{
            //    string scannedData = "";
            //    foreach (BarcodeQrData barcodeQrData in data)
            //    {
            //        scannedData += barcodeQrData.text + "\n";
            //    }
            //    ResultLabel.Text = scannedData;
            //}
            //else
            //{
            //    ResultLabel.Text = "";
            //}
        }

        void OnCanvasViewPaintSurface(object sender, SKPaintSurfaceEventArgs args)
        {
            double width = canvasView.Width;
            double height = canvasView.Height;

            var mainDisplayInfo = DeviceDisplay.MainDisplayInfo;
            var orientation = mainDisplayInfo.Orientation;
            var rotation = mainDisplayInfo.Rotation;
            var density = mainDisplayInfo.Density;

            width *= density;
            height *= density;

            SKImageInfo info = args.Info;
            SKSurface surface = args.Surface;
            SKCanvas canvas = surface.Canvas;

            canvas.Clear();

            SKPaint skPaint = new SKPaint
            {
                Style = SKPaintStyle.Stroke,
                Color = SKColors.Blue,
                StrokeWidth = 10,
            };

            // Calculate the center of the screen
            float centerX = (float)(width / 2);
            float centerY = (float)(height / 2);

            // Calculate the size of the rectangle
            float rectWidth = (float)(width * 0.8); // Adjust as needed
            float rectHeight = (float)(height * 0.4); // Adjust as needed

            // Calculate the position of the rectangle
            float rectLeft = centerX - (rectWidth / 2);
            float rectTop = centerY - (rectHeight / 2);

            // Draw the rectangle
            canvas.DrawRect(rectLeft, rectTop, rectWidth, rectHeight, skPaint);
        }
    }
}
