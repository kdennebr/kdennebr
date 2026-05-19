#include <stdio.h>
#include <iostream>

using namespace std;

// Constants
const int NUM_DAYS = 5;
const int NUM_ATHLETES = 3;

// Function Prototypes
void trainingReport(int[][NUM_DAYS]);

double averageMilesAllAthletes(int[][NUM_DAYS]);

int leastNumMilesAnyAthlete(int[][NUM_DAYS]);

int mostNumMilesAnyAthlete(int[][NUM_DAYS]);

int totalMilesPerAthlete(int[][NUM_DAYS], int);

int mostMilesAthlete(int[][NUM_DAYS]);

int main() {
    
    // 2D Array Holding Mileage Data
    int milesPerAthlete[NUM_ATHLETES][NUM_DAYS];

    // Gather User Input
    for (int athlete = 0; athlete < NUM_ATHLETES; athlete++) {

        cout << "Please enter data for Athlete "
             << athlete + 1
             << " for each day:"
             << endl;

        for (int day = 0; day < NUM_DAYS; day++) {

            cout << "Day " << day + 1 << ": ";
            cin >> milesPerAthlete[athlete][day];

            // Input Validation
            while (milesPerAthlete[athlete][day] < 0) {

                cout << "ERROR: Please enter a positive mileage: ";
                cin >> milesPerAthlete[athlete][day];
            }
        }
    }

    // Display Final Training Report
    trainingReport(milesPerAthlete);

    return 0;
}

// Returns The Least Mileage Run In Any Session
int leastNumMilesAnyAthlete(int milesPerAthlete[][NUM_DAYS]) {

    int least = milesPerAthlete[0][0];

    // Step Through Entire Array
    for (int athlete = 0; athlete < NUM_ATHLETES; athlete++) {

        for (int day = 0; day < NUM_DAYS; day++) {

            if (milesPerAthlete[athlete][day] < least) {

                least = milesPerAthlete[athlete][day];
            }
        }
    }

    return least;
}

// Returns The Highest Mileage Run In Any Session
int mostNumMilesAnyAthlete(int milesPerAthlete[][NUM_DAYS]) {

    int most = milesPerAthlete[0][0];

    // Step Through Entire Array
    for (int athlete = 0; athlete < NUM_ATHLETES; athlete++) {

        for (int day = 0; day < NUM_DAYS; day++) {

            if (milesPerAthlete[athlete][day] > most) {

                most = milesPerAthlete[athlete][day];
            }
        }
    }

    return most;
}

// Returns Total Miles For One Athlete
int totalMilesPerAthlete(int milesPerAthlete[][NUM_DAYS], int athlete) {

    int total = 0;

    // Add All Days Together
    for (int day = 0; day < NUM_DAYS; day++) {

        total += milesPerAthlete[athlete][day];
    }

    return total;
}

// Returns Average Miles Across All Athletes
double averageMilesAllAthletes(int milesPerAthlete[][NUM_DAYS]) {

    int totalAll = 0;

    // Add Total Miles For Every Athlete
    for (int athlete = 0; athlete < NUM_ATHLETES; athlete++) {

        totalAll += totalMilesPerAthlete(
            milesPerAthlete,
            athlete
        );
    }

    // Convert To Double For Decimal Division
    double totalAllDouble = totalAll;
    double numAthletesDouble = NUM_ATHLETES;

    return totalAllDouble / numAthletesDouble;
}

// Returns Athlete With Most Total Miles
int mostMilesAthlete(int milesPerAthlete[][NUM_DAYS]) {

    int mostAthlete = 0;
    int totalMiles = 0;

    // Compare Total Miles Between Athletes
    for (int athlete = 0; athlete < NUM_ATHLETES; athlete++) {

        int tempTotal = totalMilesPerAthlete(
            milesPerAthlete,
            athlete
        );

        if (tempTotal > totalMiles) {

            mostAthlete = athlete;
            totalMiles = tempTotal;
        }
    }

    return mostAthlete;
}

// Displays Complete Training Report
void trainingReport(int milesPerAthlete[][NUM_DAYS]) {

    cout << endl;

    cout << "TRAINING REPORT"
         << "\n--------------------"
         << endl;

    // Display Total Miles Per Athlete
    for (int athlete = 0; athlete < NUM_ATHLETES; athlete++) {

        cout << "Total Miles Run By Athlete "
             << athlete + 1
             << ": "
             << totalMilesPerAthlete(
                    milesPerAthlete,
                    athlete
                )
             << endl;
    }

    // Display Average Mileage
    cout << "Average Miles Run By All Athletes: "
         << averageMilesAllAthletes(milesPerAthlete)
         << endl;

    // Display Lowest Mileage Session
    cout << "Least Miles Run In A Session: "
         << leastNumMilesAnyAthlete(milesPerAthlete)
         << endl;

    // Display Highest Mileage Session
    cout << "Most Miles Run In A Session: "
         << mostNumMilesAnyAthlete(milesPerAthlete)
         << endl;

    // Display Athlete With Most Miles
    cout << "Athlete "
         << mostMilesAthlete(milesPerAthlete) + 1
         << " ran the most miles over the week"
         << endl;
}
