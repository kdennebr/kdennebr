#include <stdio.h>
#include <stdbool.h>
#include <stdlib.h>
#include <time.h>

#define MIN_MILES 1.0
#define MAX_MILES 100.0
#define SENTINEL_VALUE -1.0

#define BASE_FARE 1.80
#define COST_PER_MINUTE 0.25
#define COST_PER_MILE 1.20
#define MIN_FLAT_RATE 20.00

#define MIN_RAND_MINUTES_FACTOR 1.2
#define MAX_RAND_MINUTES_FACTOR 1.5

#define MAX_RIDES 100

typedef struct {
    int rideNumber;
    double miles;
    int minutes;
    double fare;
} Ride;

/* Function prototypes */
void displayPricing(void);
void displayMenu(void);
void clearInputBuffer(void);

bool readMiles(double *miles);
bool validateMiles(double miles);

int estimateMinutes(double miles);
double calculateFare(double miles, int minutes);

void displayRide(const Ride *ride);
void displayAllRides(const Ride rides[], int rideCount);
void displaySummary(const Ride rides[], int rideCount);

double calculateTotalMiles(const Ride rides[], int rideCount);
int calculateTotalMinutes(const Ride rides[], int rideCount);
double calculateTotalRevenue(const Ride rides[], int rideCount);

int main(void) {
    Ride rides[MAX_RIDES];
    int rideCount = 0;
    int choice = 0;
    bool running = true;

    srand((unsigned int)time(NULL));

    while (running) {
        displayMenu();

        if (scanf("%d", &choice) != 1) {
            puts("\nInvalid input. Please enter a menu number.");
            clearInputBuffer();
            continue;
        }

        clearInputBuffer();

        switch (choice) {
            case 1: {
                double miles = 0.0;

                if (rideCount >= MAX_RIDES) {
                    printf("\nRide limit reached (%d rides).\n", MAX_RIDES);
                    break;
                }

                displayPricing();

                if (!readMiles(&miles)) {
                    puts("\nInvalid input. Please enter a numeric value.");
                    break;
                }

                if (miles == SENTINEL_VALUE) {
                    puts("\nReturning to main menu.");
                    break;
                }

                if (!validateMiles(miles)) {
                    printf("\nPlease enter a distance between %.0f and %.0f miles, or %.0f to cancel.\n",
                           MIN_MILES, MAX_MILES, SENTINEL_VALUE);
                    break;
                }

                rides[rideCount].rideNumber = rideCount + 1;
                rides[rideCount].miles = miles;
                rides[rideCount].minutes = estimateMinutes(miles);
                rides[rideCount].fare = calculateFare(miles, rides[rideCount].minutes);

                displayRide(&rides[rideCount]);
                rideCount++;
                break;
            }

            case 2:
                displayAllRides(rides, rideCount);
                break;

            case 3:
                displaySummary(rides, rideCount);
                break;

            case 4:
                puts("\nExiting program. Final summary:");
                displaySummary(rides, rideCount);
                running = false;
                break;

            default:
                puts("\nPlease choose a valid option (1-4).");
        }
    }

    return 0;
}

void displayPricing(void) {
    puts("\nUCCS Ride Share Pricing");
    puts("-----------------------");
    printf("Service range: %.0f to %.0f miles\n", MIN_MILES, MAX_MILES);
    printf("Base fare: $%.2f\n", BASE_FARE);
    printf("Cost per minute: $%.2f\n", COST_PER_MINUTE);
    printf("Cost per mile: $%.2f\n", COST_PER_MILE);
    printf("Minimum flat rate: $%.2f\n", MIN_FLAT_RATE);
    printf("Enter %.0f to cancel and return to the menu.\n", SENTINEL_VALUE);
}

void displayMenu(void) {
    puts("\n==============================");
    puts("      UCCS Ride Share");
    puts("==============================");
    puts("1. Add a ride");
    puts("2. View all rides");
    puts("3. View business summary");
    puts("4. Exit");
    printf("Enter choice: ");
}

void clearInputBuffer(void) {
    int ch;
    while ((ch = getchar()) != '\n' && ch != EOF) {
        /* discard remaining input */
    }
}

bool readMiles(double *miles) {
    printf("\nEnter number of miles: ");

    if (scanf("%lf", miles) != 1) {
        clearInputBuffer();
        return false;
    }

    clearInputBuffer();
    return true;
}

bool validateMiles(double miles) {
    return ((miles >= MIN_MILES && miles <= MAX_MILES) || miles == SENTINEL_VALUE);
}

int estimateMinutes(double miles) {
    double minMinutes = MIN_RAND_MINUTES_FACTOR * miles;
    double maxMinutes = MAX_RAND_MINUTES_FACTOR * miles;
    double randomScale = (double)rand() / (double)RAND_MAX;
    double estimated = minMinutes + randomScale * (maxMinutes - minMinutes);

    return (int)(estimated + 0.5);
}

double calculateFare(double miles, int minutes) {
    double fare = BASE_FARE + (miles * COST_PER_MILE) + (minutes * COST_PER_MINUTE);

    if (fare < MIN_FLAT_RATE) {
        fare = MIN_FLAT_RATE;
    }

    return fare;
}

void displayRide(const Ride *ride) {
    puts("\nRide Added");
    puts("----------------------------------------------------------------");
    puts("Ride #   Miles      Minutes    Fare");
    printf("%-8d %-10.2f %-10d $%.2f\n",
           ride->rideNumber, ride->miles, ride->minutes, ride->fare);
}

void displayAllRides(const Ride rides[], int rideCount) {
    int i;

    puts("\nAll Ride Records");
    puts("----------------------------------------------------------------");

    if (rideCount == 0) {
        puts("No rides recorded.");
        return;
    }

    puts("Ride #   Miles      Minutes    Fare");
    for (i = 0; i < rideCount; i++) {
        printf("%-8d %-10.2f %-10d $%.2f\n",
               rides[i].rideNumber, rides[i].miles, rides[i].minutes, rides[i].fare);
    }
}

void displaySummary(const Ride rides[], int rideCount) {
    double totalMiles;
    int totalMinutes;
    double totalRevenue;
    double avgMiles;
    double avgMinutes;
    double avgFare;

    puts("\nBusiness Summary");
    puts("----------------------------------------------------------------");

    if (rideCount == 0) {
        puts("No rides to summarize.");
        return;
    }

    totalMiles = calculateTotalMiles(rides, rideCount);
    totalMinutes = calculateTotalMinutes(rides, rideCount);
    totalRevenue = calculateTotalRevenue(rides, rideCount);

    avgMiles = totalMiles / rideCount;
    avgMinutes = (double)totalMinutes / rideCount;
    avgFare = totalRevenue / rideCount;

    printf("Total rides: %d\n", rideCount);
    printf("Total miles: %.2f\n", totalMiles);
    printf("Total minutes: %d\n", totalMinutes);
    printf("Total revenue: $%.2f\n", totalRevenue);
    printf("Average miles per ride: %.2f\n", avgMiles);
    printf("Average minutes per ride: %.2f\n", avgMinutes);
    printf("Average fare per ride: $%.2f\n", avgFare);
}

double calculateTotalMiles(const Ride rides[], int rideCount) {
    double total = 0.0;
    int i;

    for (i = 0; i < rideCount; i++) {
        total += rides[i].miles;
    }

    return total;
}

int calculateTotalMinutes(const Ride rides[], int rideCount) {
    int total = 0;
    int i;

    for (i = 0; i < rideCount; i++) {
        total += rides[i].minutes;
    }

    return total;
}

double calculateTotalRevenue(const Ride rides[], int rideCount) {
    double total = 0.0;
    int i;

    for (i = 0; i < rideCount; i++) {
        total += rides[i].fare;
    }

    return total;
}
