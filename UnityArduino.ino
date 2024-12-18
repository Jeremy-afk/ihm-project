const int redPin = 9;  // Broche pour la LED rouge
const int greenPin = 10; // Broche pour la LED verte

void setup() {
  pinMode(redPin, OUTPUT);
  pinMode(greenPin, OUTPUT);
  Serial.begin(9600); // Initialiser la communication série
}

void loop() {
  if (Serial.available() > 0) {
  String data = Serial.readStringUntil('\n'); // Lire les données envoyées par Unity
    float barValue = data.toFloat(); // Convertir les données en float (valeurs entre 0.0 et 1.0)

    // Calculer les intensités lumineuses
    int redIntensity = barValue < 0.5 ? (1.0 - barValue * 2.0) * 255 : 0;
    int greenIntensity = barValue >= 0.5 ? (barValue - 0.5) * 2.0 * 255 : 0;

    // Appliquer les intensités aux LED
    analogWrite(redPin, redIntensity);
    analogWrite(greenPin, greenIntensity);
  }
}
