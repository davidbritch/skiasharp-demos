using SkiaSharp.Views.Maui;
using SkiaSharp;

namespace PathFlattening;

public partial class MainPage : ContentPage
{
    SKPath globePath;

    public MainPage()
    {
        InitializeComponent();        

        using (SKFont font = new SKFont())
        {
            font.Typeface = SKTypeface.FromFamilyName("Times New Roman");
            font.Size = 100;

            using (SKPath textPath = font.GetTextPath("SKIASHARP", new SKPoint(0, 0)))
            {
                SKRect textPathBounds;
                textPath.GetBounds(out textPathBounds);

                globePath = textPath.CloneWithTransform((SKPoint pt) =>
                {
                    double longitude = (Math.PI / textPathBounds.Width) * (pt.X - textPathBounds.Left) - Math.PI / 2;
                    double latitude = (Math.PI / textPathBounds.Height) * (pt.Y - textPathBounds.Top) - Math.PI / 2;

                    longitude *= 0.75;
                    latitude *= 0.75;

                    float x = (float)(Math.Cos(latitude) * Math.Sin(longitude));
                    float y = (float)Math.Sin(latitude);

                    return new SKPoint(x, y);
                });
            }
        }
    }

    void OnCanvasViewPaintSurface(object sender, SKPaintSurfaceEventArgs e)
    {
        SKImageInfo info = e.Info;
        SKSurface surface = e.Surface;
        SKCanvas canvas = surface.Canvas;

        canvas.Clear();

        using (SKPaint pathPaint = new SKPaint())
        {
            pathPaint.Style = SKPaintStyle.Fill;
            pathPaint.Color = SKColors.Blue;
            pathPaint.StrokeWidth = 3;
            pathPaint.IsAntialias = true;

            canvas.Translate(info.Width / 2, info.Height / 2);
            canvas.Scale(0.45f * Math.Min(info.Width, info.Height)); // Radius
            canvas.DrawPath(globePath, pathPaint);
        }        
	}
}
