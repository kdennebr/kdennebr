package defaultpackage;
/*Kayden Denne-Brown
 * Read Values from a file and create a stack with those values. Then create methods to displayStack, mergeStack, and findSecondLargestValue. 
 * Then create a class called Generic Class for creating Generic stacks, create all the associated methods like pop, push, isEmpty, and peek. 
 * Merge Stacks and Display Duplicate Values in the stack for all 4 stacks.
 */

import java.util.ArrayList;
import java.util.Stack;
import java.io.File;
import java.io.IOException;
import java.util.Scanner;

public class DenneBrownKaydenAssignment5 {

	public static void main(String[] args) throws IOException {
		
		//Part 1
		int[] values = {10,27,19,45,4,2,82,48};
		
		Stack<Integer> stack1 = new Stack<Integer>();
		
		for(int i = 0; i < values.length; i++) {
			stack1.push(values[i]);
		}
	
		System.out.printf("Stack Values and Second Largest Value\n-------------------------------------------\n");
		printStack(stack1);
		
		System.out.println("\nSecond Largest Value: " + findSecondLargest(stack1));
		
		//Part 2
		
		//numbers1.txt
		Scanner readNumbers1 = new Scanner(new File("numbers1.txt"));
		
		GenericStack<Integer> numStack1 = new GenericStack<>();
		
		while(readNumbers1.hasNext()) {
			numStack1.push(readNumbers1.nextInt());
		}
		
		//numbers2.txt
		Scanner readNumbers2 = new Scanner(new File("numbers2.txt"));
		
		GenericStack<Integer> numStack2 = new GenericStack<>();
		
		while(readNumbers2.hasNext()) {
			numStack2.push(readNumbers2.nextInt());
		}
		
		System.out.printf("\nNumbers Stack 1 - filled with values from numbers1.txt\n----------------------------------------\n");
		
		printStack(numStack1);
		
		System.out.printf("\nNumbers Stack 2 - filled with values from numbers2.txt\n----------------------------------------\n");
		
		printStack(numStack2);
		
		//Merged Stack
		GenericStack<Integer> mergedStack = new GenericStack<>();
		
		mergeStacks(numStack1, numStack2, mergedStack);
		
		System.out.printf("\nMerged Stack - largest value on top\n---------------------------------\n");
		
		printStack(mergedStack);
		
		System.out.printf("\nDetails About Duplicate Values\n-----------------------\n");
		
		displayDuplicateCount(mergedStack);
		
		
		//mountains1.txt
		Scanner readMountains1 = new Scanner(new File("mountains1.txt"));
		
		GenericStack<String> mountainStack1 = new GenericStack<>();
		
		while(readMountains1.hasNext()) {
			mountainStack1.push(readMountains1.nextLine());
		}
		
		
		//mountains2.txt
		Scanner readMountains2 = new Scanner(new File("mountains2.txt"));
		
		GenericStack<String> mountainStack2 = new GenericStack<>();
		
		while(readMountains2.hasNext()) {
			mountainStack2.push(readMountains2.nextLine());
		}
		
		System.out.printf("\nString stack 1 - filled with values from mountains1.txt\n-----------------------------------\n");
		
		printStack(mountainStack1);
		
		System.out.printf("\nString stack 2 - filled with values from mountains2.txt\n-----------------------------------\n");
		
		printStack(mountainStack2);
		
		
		//merged stack
		GenericStack<String> mountainStackMerged = new GenericStack<>();
		
		mergeStacks(mountainStack1, mountainStack2, mountainStackMerged);
		
		System.out.printf("\nMerged Stack - largest value on top\n-----------------------------------\n");
		
		printStack(mountainStackMerged);
		
		System.out.printf("\nDetails about duplicate values\n--------------------------\n");
		
		displayDuplicateCount(mountainStackMerged);
		
		
	}
	
	//Iterates Through Stack and Finds The greatest value greater than the largest value
	static int findSecondLargest(Stack<Integer> stack) {
		//Temp Stack to hold values
		Stack<Integer> tempStack = new Stack<Integer>();

		//Initializing Largest and Second largest Values
		int largest = stack.pop();
		int secondLargest = stack.pop();
		
		//If the initialized larget value is less than the second largest, they switch
		if(largest < secondLargest) {
			int temp = largest;
			largest = secondLargest;
			secondLargest = temp;
		}
		
		//Run until stack is empty
		while(!stack.isEmpty()) {
			//Gets current value in stack
			int currentVal = stack.pop();
			
			//if this value is greater than the largest value, then the largest value becomes the second largest, and the current val becomes the largest
			if(currentVal > largest) {
				secondLargest = largest;
				largest = currentVal;
			}
			//if the current value is less than the largest but greater than the second largest, the current val is now the second largest
			else if(currentVal > secondLargest && currentVal != largest) {
				secondLargest = currentVal;
			}
			
			//pushes value onto temp stack
			tempStack.push(currentVal);
		}
		
		//returns all values to original stack
		while(!tempStack.isEmpty()) {
			stack.push(tempStack.pop());
		}
		
		//returns the second largest value
		return secondLargest;
	}
	
	//Prints Stack
	public static void printStack(Stack<Integer> stack) {
		//Creates a temp stack to hold values
		Stack<Integer> tempStack = new Stack<Integer>();
		
		//run until stack is empty
		while(!stack.isEmpty()) {
			//value to be printed is the current value in stack
			int val = stack.pop();
			//value is pushed to temp stack
			tempStack.push(val);
			//value is printed
			System.out.println(val);
			
		}
		
		//Return All values from temp stack to original stack
		while(!tempStack.isEmpty()) {
			stack.push(tempStack.pop());
		}
	}
	
	//Generic Print Stack Method
	public static <E> void printStack(GenericStack<E> stack) {
		//Creates a generic temp stack to hold values
		GenericStack<E> tempStack = new GenericStack<E>();
		
		//Run until given stack is empty
		while(!stack.isEmpty()) {
			//Value to be printed is the current value in stack
			E val = stack.pop();
			//Pushes value to temp stack
			tempStack.push(val);
			//Prints Value
			System.out.println(val);
			
		}
		
		//Returns all values on temp stack to original stack
		while(!tempStack.isEmpty()) {
			stack.push(tempStack.pop());
		}
	}
	
	//Merges two generic stacks
	public static <E extends Comparable<E>> void mergeStacks(GenericStack<E> stack1, GenericStack<E> stack2, GenericStack<E> mergedStack){

		//Run until either stack is empty
		while(!stack1.isEmpty() || !stack2.isEmpty()){
			//If "stack1" is empty first, add value from stack 2 onto merged stack
			if(stack1.isEmpty()) {
				mergedStack.push(stack2.pop());
			}
			//if stack 2 is empty first, add value from stack 1 onto merged stack
			else if(stack2.isEmpty()) {
				mergedStack.push(stack1.pop());
			}
			else {
				//If stack 1 top value is greater than stack 2 value, push stack one val onto main stack
				if(stack1.peek().compareTo(stack2.peek()) <= 0) {
					mergedStack.push(stack1.pop());
				}
				else {
					//Push stack 2 value onto merged stack
					mergedStack.push(stack2.pop());
				}
			}
		}

	}
	
	//Displays duplicate values in stacks
	public static <E extends Comparable<E>> void displayDuplicateCount(GenericStack<E> duplicatesStack) {
	   //Creates a temp stack to hold values
		GenericStack<E> tempStack = new GenericStack<>();

	    //Until given stack is empty
		while (!duplicatesStack.isEmpty()) {
	       //Generic duplicate value(placeholder) is whatever is on top of stack
			E duplicateValue = duplicatesStack.pop();
	        //the amount of times that value is seen starts at 1
			int duplicateCount = 1;

	        // Until given stack is empty
	        while (!duplicatesStack.isEmpty()) {
	            //the current value equals whatever is on top of stack
	        	E currentValue = duplicatesStack.pop();

	        	//if the current value is the duplicate value
	            if (currentValue.compareTo(duplicateValue) == 0) {
	            	duplicateCount++;
	            	tempStack.push(currentValue);
	            } else {
	                //if it isn't add it to temp stack (just to ensure original stack isn't modified)
	            	tempStack.push(currentValue);
	            }
	        }

	        // Display amount of duplicates and what their values are
	        if (duplicateCount > 1) {
	            System.out.println(duplicateValue + ": " + duplicateCount);
	        }

	        // Return original stack to normal
	        while (!tempStack.isEmpty()) {
	            duplicatesStack.push(tempStack.pop());
	        }
	    }
			
			
		}
}



class GenericStack<E>{
	
	private ArrayList<E> list;
	
	public GenericStack() {
		list = new ArrayList<>();
	}
	
	public boolean isEmpty() {
		boolean empty = list.isEmpty();
		
		return empty;
	}
	
	public int getSize() {
		int size = list.size();
		
		return size;
	}
	
	public E peek(){
		return list.get(getSize()-1);
		
	}
	
	public E pop() {
		return list.remove(getSize()-1);
	}
	
	public void push(E value) {
		list.add(value);
	}
}

