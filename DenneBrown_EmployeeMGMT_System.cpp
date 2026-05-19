#include <iostream>
#include <string>

using namespace std;

// Employee Base Class
class Employee {
private:
    string name;
    int employeeNumber;
    string hireDate;
    bool disabled;

public:
    
    // Default Constructor
    Employee()
        : name(""), employeeNumber(0), hireDate(""), disabled(false) {}
    
    // Constructor With Parameters
    Employee(string n, int num, string date, bool dis)
        : name(n), employeeNumber(num), hireDate(date), disabled(dis) {}
    
    // Getters
    string getName() const {
        return name;
    }

    int getEmployeeNumber() const {
        return employeeNumber;
    }

    string getHireDate() const {
        return hireDate;
    }

    bool isDisabled() const {
        return disabled;
    }

    // Setters
    void setName(string n) {
        name = n;
    }

    void setEmployeeNumber(int num) {
        employeeNumber = num;
    }

    void setHireDate(string date) {
        hireDate = date;
    }

    void setDisabled(bool dis) {
        disabled = dis;
    }
};

// Derived ProductionWorker Class
class ProductionWorker : public Employee {
private:
    int shift;
    double hourlyPayRate;

public:
    
    // Default Constructor
    ProductionWorker()
        : Employee(), shift(1), hourlyPayRate(0.0) {}
    
    // Constructor With Parameters
    ProductionWorker(string n, int num, string date,
                     bool dis, int s, double payRate)
        : Employee(n, num, date, dis),
          shift(s),
          hourlyPayRate(payRate) {}
    
    // Getters
    int getShift() const {
        return shift;
    }

    double getHourlyPayRate() const {
        return hourlyPayRate;
    }

    // Setters
    void setShift(int s) {
        shift = s;
    }

    void setHourlyPayRate(double payRate) {
        hourlyPayRate = payRate;
    }

    // Returns Shift Name Instead Of Number
    string getShiftName() const {
        if (shift == 1) {
            return "Day";
        }
        else if (shift == 2) {
            return "Night";
        }
        else {
            return "Invalid";
        }
    }
};

int main() {
    
    // Create Production Worker Object
    ProductionWorker worker(
        "John Smith",
        12345,
        "05/12/2025",
        false,
        1,
        24.75
    );

    // Output Employee Information
    cout << "EMPLOYEE INFORMATION" << endl;
    cout << "---------------------" << endl;

    cout << "Name: "
         << worker.getName() << endl;

    cout << "Employee Number: "
         << worker.getEmployeeNumber() << endl;

    cout << "Hire Date: "
         << worker.getHireDate() << endl;

    cout << "Shift: "
         << worker.getShiftName() << endl;

    cout << "Hourly Pay Rate: $"
         << worker.getHourlyPayRate() << endl;

    // Displays Disabled Status
    if (worker.isDisabled()) {
        cout << "Disabled: Yes" << endl;
    }
    else {
        cout << "Disabled: No" << endl;
    }

    return 0;
}
