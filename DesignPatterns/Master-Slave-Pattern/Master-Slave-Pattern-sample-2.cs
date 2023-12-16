using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace concurrentDesignPatterns.DesignPatterns.Master_Slave_Pattern
{
    internal class Master_Slave_Pattern_sample_2
    {
        //static async Task Main()
        //{
        //    // add your image path
        //    string inputImagePath = "C:\\Users\\NirajRanasinghe\\sample-image.png"; 

        //    // add a path for the generated output image
        //    string outputImagePath = "C:\\Users\\NirajRanasinghe\\sample-image-output.png"; 

        //    using (var originalImage = LoadImage(inputImagePath))
        //    {
        //        // Adjust based on available processors
        //        int numSegments = Environment.ProcessorCount * 2; 
        //        var processedSegments = await MasterProcessAsync(originalImage, numSegments, CancellationToken.None);
        //        var finalResult = await AggregateResultsAsync(processedSegments);

        //        // Save or display the final processed image
        //        await SaveImageAsync(finalResult, outputImagePath);
        //        Console.WriteLine("Image processing completed successfully.");
        //    }
        //}

        static Bitmap LoadImage(string filePath)
        {
            // Load the image from file
            return new Bitmap(filePath);
        }

        static async Task<List<Bitmap>> MasterProcessAsync(Bitmap originalImage, int numSegments, CancellationToken cancellationToken)
        {
            int segmentHeight = originalImage.Height / numSegments;
            var tasks = new List<Task<Bitmap>>();

            for (int i = 0; i < numSegments; i++)
            {
                int startY = i * segmentHeight;
                int endY = (i == numSegments - 1) ? originalImage.Height : (i + 1) * segmentHeight;

                Bitmap imageSegment = originalImage.Clone(new Rectangle(0, startY, originalImage.Width, endY - startY),
                                                          originalImage.PixelFormat);

                tasks.Add(ProcessSegmentAsync(imageSegment, cancellationToken));
            }

            Bitmap[] results = await Task.WhenAll(tasks);
            // Convert the array to a List<Bitmap>
            return results.ToList(); 
        }

        static async Task<Bitmap> ProcessSegmentAsync(Bitmap imageSegment, CancellationToken cancellationToken)
        {
            // Simulate asynchronous image processing (applying a filter in this example)
            await Task.Run(() => ApplyFilterAsync(imageSegment, cancellationToken));
            Console.WriteLine($"Slave Processed a segment asynchronously. Thread ID: {Thread.CurrentThread.ManagedThreadId}");
            return imageSegment;
        }

        static async Task ApplyFilterAsync(Bitmap image, CancellationToken cancellationToken)
        {
            // Simulate asynchronous image filter application (grayscale conversion in this example)
            await Task.Run(() =>
            {
                for (int x = 0; x < image.Width; x++)
                {
                    for (int y = 0; y < image.Height; y++)
                    {
                        // Check for cancellation before processing each pixel
                        cancellationToken.ThrowIfCancellationRequested();

                        Color pixel = image.GetPixel(x, y);
                        int average = (pixel.R + pixel.G + pixel.B) / 3;
                        image.SetPixel(x, y, Color.FromArgb(average, average, average));
                    }
                }
            }, cancellationToken);
        }

        static async Task SaveImageAsync(Bitmap image, string filePath)
        {
            await Task.Run(() => image.Save(filePath));
        }

        static async Task<Bitmap> AggregateResultsAsync(IEnumerable<Bitmap> processedSegments)
        {
            int totalHeight = processedSegments.Sum(segment => segment.Height);
            var finalResult = new Bitmap(processedSegments.First().Width, totalHeight);

            using (Graphics g = Graphics.FromImage(finalResult))
            {
                int yOffset = 0;

                foreach (var segment in processedSegments)
                {
                    // Draw each segment at the appropriate yOffset without black stripes
                    g.DrawImage(segment, new Rectangle(0, yOffset, finalResult.Width, segment.Height));
                    yOffset += segment.Height;
                }
            }

            return finalResult;
        }
    }
}
