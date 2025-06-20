using System.IO;
using System.Windows;
using MultiFinger.Models;
using MultiFinger.Models.Figures;
using MultiFinger.Models.Interfaces;

namespace MultiFinger.Services
{
    public class TextFileService : ITextFileService
    {
        // Color constants in ARGB string format
        private const string BlueColor = "255; 0; 0; 255";
        private const string GreenColor = "255; 2; 249; 77";
        private const string RedColor = "255; 255; 0; 0";

        public async Task<Dictionary<FingerTrace, List<FigureBase>>> ParseFileAsync(string path)
        {
            string filePath = string.IsNullOrWhiteSpace(path) ? "C:\\Temp\\data.txt" : path;
            var sampleFigures = new Dictionary<FingerTrace, List<FigureBase>>();

            const int count = 40;
            var samples = new Dictionary<int, List<double>>();

            try
            {
                int countOfSamples = 0;
                using (var reader = new StreamReader(filePath))
                {
                    while (!reader.EndOfStream)
                    {
                        string? line = await reader.ReadLineAsync();
                        if (string.IsNullOrWhiteSpace(line))
                            continue;

                        string[] parts = line.Split(' ');
                        var distances = new List<double>();

                        foreach (string part in parts)
                        {
                            if (double.TryParse(part, out double num))
                            {
                                distances.Add(num);
                            }
                        }

                        countOfSamples++;
                        samples.Add(countOfSamples, distances);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error reading file: {ex.Message}");
                return sampleFigures;
            }

            for (int i = 0; i < samples.Count; i++)
            {
                double sumX = 0, sumY = 0;
                var distances = samples.ElementAt(i).Value;
                var points = new List<FigurePoint>();

                for (int j = 0; j < count; j++)
                {
                    double angle = 2 * Math.PI * j / count;
                    double x = distances[j] * Math.Cos(angle);
                    double y = distances[j] * Math.Sin(angle);
                    points.Add(new FigurePoint()
                    {
                        A = new Point((int)x, (int)y),
                        ArgbColor = BlueColor,
                        ToolTip = $"X: {x}, Y: {y}"
                    });

                    sumX += x;
                    sumY += y;
                }

                double offsetX = sumX / count;
                double offsetY = sumY / count;
                double offsetMagnitude = Math.Sqrt(offsetX * offsetX + offsetY * offsetY);

                var offsetPoint = new Point((int)offsetX, (int)offsetY);
                var lines = new List<Line>();

                foreach (var point in points)
                {
                    lines.Add(new Line()
                    {
                        A = point.A,
                        B = offsetPoint,
                        ArgbColor = GreenColor,
                        ToolTip = $"X: {point.A.X}, Y: {point.A.Y} - X: {offsetPoint.X}, Y: {offsetPoint.Y}"
                    });
                }

                var transformedPoints = new List<FigurePoint>();

                for (int j = 0; j < count; j++)
                {
                    double angle = 2 * Math.PI * j / count;
                    double x = distances[j] * Math.Cos(angle);
                    double y = distances[j] * Math.Sin(angle);

                    double correctedX = x - offsetX;
                    double correctedY = y - offsetY;

                    transformedPoints.Add(new FigurePoint()
                    {
                        A = new Point((int)correctedX, (int)correctedY),
                        ArgbColor = BlueColor,
                        ToolTip = $"X: {x}, Y: {y}",
                        IsTransformed = true
                    });
                }

                var transformedOffsetPoint = new Point(0, 0);
                var transformedLines = new List<Line>();

                foreach (var point in transformedPoints)
                {
                    transformedLines.Add(new Line()
                    {
                        A = point.A,
                        B = transformedOffsetPoint,
                        ArgbColor = GreenColor,
                        ToolTip = $"X: {point.A.X}, Y: {point.A.Y} - X: {offsetPoint.X}, Y: {offsetPoint.Y}",
                        IsTransformed = true
                    });
                }

                var figures = new List<FigureBase>();
                figures.AddRange(lines);
                figures.AddRange(transformedLines);
                figures.AddRange(points);
                figures.AddRange(transformedPoints);
                figures.Add(new FigurePoint()
                {
                    A = offsetPoint,
                    ArgbColor = RedColor,
                });
                figures.Add(new FigurePoint()
                {
                    A = transformedOffsetPoint,
                    ArgbColor = RedColor,
                    IsTransformed = true
                });

                var fingerTrace = new FingerTrace()
                {
                    Number = i,
                    OffsetFromCenter = offsetMagnitude,
                };

                sampleFigures.Add(fingerTrace, figures);
            }

            return sampleFigures;
        }

        public async Task<bool> SaveFileAsync(string path)
        {
            string filePath = string.IsNullOrWhiteSpace(path) ? "C:\\Temp\\data.txt" : path;

            const int count = 40;
            var samples = new Dictionary<int, List<double>>();

            try
            {
                int countOfSamples = 0;
                using (var reader = new StreamReader(filePath))
                {
                    while (!reader.EndOfStream)
                    {
                        string? line = await reader.ReadLineAsync();
                        if (string.IsNullOrWhiteSpace(line))
                            continue;

                        string[] parts = line.Split(' ');
                        var distances = new List<double>();

                        foreach (string part in parts)
                        {
                            if (double.TryParse(part, out double num))
                            {
                                distances.Add(num);
                            }
                        }

                        countOfSamples++;
                        samples.Add(countOfSamples, distances);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error reading file: {ex.Message}");
                return false;
            }

            // Prepare lines to write
            var linesToWrite = new List<string>();

            for (int i = 0; i < samples.Count; i++)
            {
                double sumX = 0, sumY = 0;
                var distances = samples.ElementAt(i).Value;

                for (int j = 0; j < count; j++)
                {
                    double angle = 2 * Math.PI * j / count;
                    double x = distances[j] * Math.Cos(angle);
                    double y = distances[j] * Math.Sin(angle);

                    sumX += x;
                    sumY += y;
                }

                double offsetX = sumX / count;
                double offsetY = sumY / count;
                double offsetMagnitude = Math.Sqrt(offsetX * offsetX + offsetY * offsetY);

                var correctedDistances = new List<double>();

                for (int j = 0; j < count; j++)
                {
                    double angle = 2 * Math.PI * j / count;

                    double x = distances[j] * Math.Cos(angle);
                    double y = distances[j] * Math.Sin(angle);

                    double correctedX = x - offsetX;
                    double correctedY = y - offsetY;

                    double lengthFromCorrectedCenter = Math.Round(Math.Sqrt(correctedX * correctedX + correctedY * correctedY), 2);
                    correctedDistances.Add(lengthFromCorrectedCenter);
                }

                // Join correctedDistances as a space-separated string
                string line = string.Join(" ", correctedDistances.Select(d => d.ToString("F2")));
                linesToWrite.Add(line);
            }

            // Write to a new file (output file)
            string outputFilePath = Path.Combine(
                Path.GetDirectoryName(filePath) ?? "",
                Path.GetFileNameWithoutExtension(filePath) + "_transformed" + Path.GetExtension(filePath)
            );

            try
            {
                await File.WriteAllLinesAsync(outputFilePath, linesToWrite);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error writing file: {ex.Message}");
                return false;
            }

            return true;
        }
    }
}
