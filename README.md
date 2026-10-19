# Практическая работа №7: Пользовательские методы в C#, передача параметров (ref, out, params), перегрузка и декомпозиция сложных систем
# Вариант 10. Планетарный сейсмограф (База на Луне)
## Выполнили студенты группы П25-2.1 Карпухин Дмитрий и Есин Данила
---
<picture>
  <img src="https://github.com/Karpukhin-D/Task-7/blob/main/1.png">
</picture>

```csharp

using System;

class Program
{
    static void Main()
    {
        Console.WriteLine("=== ПЛАНЕТАРНЫЙ СЕЙСМОГРАФ ===");
        Console.WriteLine("        База на Луне\n       ");

        // Задание 1. Запись сейсмического импульса
        int energyStorageJoules = 8000;
        double baseNoiseLevel = 1.2;
        double richterMagnitude;
        bool moonquakeDetected;

        RecordSeismicImpulse(ref energyStorageJoules, ref baseNoiseLevel,
            out richterMagnitude, out moonquakeDetected);

        Console.WriteLine($"Магнитуда: {richterMagnitude:F2}");
        Console.WriteLine($"Остаток энергии: {energyStorageJoules} Дж");
        Console.WriteLine($"Глубокое лунотрясение: {moonquakeDetected}\n");

        // Задание 2. Фильтрация акустического эха
        double signalAmplitude = 100;
        FilterAcousticEcho(ref signalAmplitude);
        Console.WriteLine($"Амплитуда после фильтрации: {signalAmplitude:F2}");

        double coreReflectionIndex = 0.8;
        double pWaveSpeed, sWaveSpeed;
        FilterAcousticEcho(ref signalAmplitude, coreReflectionIndex,
            out pWaveSpeed, out sWaveSpeed);

        Console.WriteLine($"Скорость P-волны: {pWaveSpeed:F0} м/с");
        Console.WriteLine($"Скорость S-волны: {sWaveSpeed:F0} м/с\n");

        // Задание 3. Рекурсивное затухание ударной волны
        double remainingEnergy = TraceCrustShockwave(10000, 2.5, 5);
        Console.WriteLine($"Энергия ударной волны после затухания: {remainingEnergy:F2} Дж\n");

        // Задание 4. Суммарное смещение геофонов
        double displacement = CalculateDisplacementVector(0.5, 1.2, -0.3, 2.5, 0.2, -1.1);
        Console.WriteLine($"Суммарное смещение коры: {displacement:F2} мкм\n");

        // Задание 5. Анализ матрицы сейсмодатчиков
        double[,] sensorGrid =
        {
            { 1.2, 2.1, 1.5, 0.8, 1.1 },
            { 2.0, 3.4, 2.2, 1.0, 0.7 },
            { 1.4, 2.5, 5.8, 2.3, 1.2 },
            { 0.9, 1.3, 2.7, 3.1, 1.5 },
            { 0.6, 0.8, 1.2, 1.4, 2.0 }
        };

        AnalyzeSeismicArrayMatrix(sensorGrid);
    }

    // Задание 1
    static void RecordSeismicImpulse(ref int energyStorageJoules,
        ref double baseNoiseLevel, out double richterMagnitude,
        out bool moonquakeDetected)
    {
        if (energyStorageJoules <= 0)
        {
            richterMagnitude = 0;
            moonquakeDetected = false;
            return;
        }

        richterMagnitude = energyStorageJoules / 1000.0 - baseNoiseLevel;
        moonquakeDetected = richterMagnitude >= 2.5;

        energyStorageJoules = energyStorageJoules / 2;
        baseNoiseLevel = baseNoiseLevel + 0.1;
    }

    // Задание 2. Перегрузка 1
    static void FilterAcousticEcho(ref double signalAmplitude)
    {
        signalAmplitude = signalAmplitude * 0.5;
    }

    // Задание 2. Перегрузка 2
    static void FilterAcousticEcho(ref double signalAmplitude,
        double coreReflectionIndex, out double pWaveSpeed, out double sWaveSpeed)
    {
        signalAmplitude = signalAmplitude * coreReflectionIndex;
        pWaveSpeed = 6000 * coreReflectionIndex;
        sWaveSpeed = 3500 * coreReflectionIndex;
    }

    // Задание 3
    static double TraceCrustShockwave(double initialJoules,
        double regolithDensity, int radiusKm)
    {
        if (radiusKm <= 0 || initialJoules < 1)
            return 0;

        double remainingEnergy = initialJoules / (1 + regolithDensity);

        return TraceCrustShockwave(remainingEnergy, regolithDensity, radiusKm - 1);
    }

    // Задание 4
    static double CalculateDisplacementVector(double faultTolerance,
        params double[] sensorOffsets)
    {
        double total = 0;

        for (int i = 0; i < sensorOffsets.Length; i++)
        {
            if (Math.Abs(sensorOffsets[i]) >= faultTolerance)
                total = total + sensorOffsets[i];
        }

        return total;
    }

    // Задание 5
    static void AnalyzeSeismicArrayMatrix(double[,] sensorGrid)
    {
        int rows = sensorGrid.GetLength(0);
        int cols = sensorGrid.GetLength(1);

        double maxAmplitude = sensorGrid[0, 0];
        int maxRow = 0;
        int maxCol = 0;
        bool diagonalsIntact = true;

        Console.WriteLine("Карта вибраций:");

        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < cols; j++)
            {
                if (sensorGrid[i, j] > maxAmplitude)
                {
                    maxAmplitude = sensorGrid[i, j];
                    maxRow = i;
                    maxCol = j;
                }

                if (sensorGrid[i, j] <= 0)
                    diagonalsIntact = false;

                if (sensorGrid[i, j] >= 3)
                    Console.ForegroundColor = ConsoleColor.Red;
                else
                    Console.ForegroundColor = ConsoleColor.Green;

                Console.Write($"{sensorGrid[i, j],5:F1}");
                Console.ResetColor();
            }

            Console.WriteLine();
        }

        Console.WriteLine($"Эпицентр: строка {maxRow}, столбец {maxCol}");
        Console.WriteLine($"Максимальная амплитуда: {maxAmplitude:F1}");
        Console.WriteLine($"Диагонали датчиков целы: {diagonalsIntact}");
    }
}
```
