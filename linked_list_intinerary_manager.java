package defaultpackage;

import java.util.Scanner;
import java.io.File;
import java.io.IOException;

/*Kayden Denne-Brown
 * Read through a file and place those values in a single linked list, then remove all the adventure type from the single linked list and add it to the double linked list. Print this double linked list in reverse order and then print the single linked list in reverse order.
 */

public class DenneBrownKaydenAssignment9 {

	
	public static void main(String[] args) throws IOException{
		//Initializing
		ItineraryLinkedList singleLinkedList = new ItineraryLinkedList();
		DoubleLinkedList doubleLinkedList = new DoubleLinkedList();
		
		Scanner readFile = new Scanner(new File("ParisItinerary.txt"));
		
		//Reading Values From File
		while(readFile.hasNext()) {
			int stop = readFile.nextInt();
			String type = readFile.next();
			String name = readFile.next();
			String activity = readFile.nextLine();
			Destination destination = new Destination(stop, type, activity);
			destination.setName(name);
			singleLinkedList.addByStopNumber(destination);
		}
		//Printing Out Single Linked List With all the Values in it
		System.out.println("Sorted Paris Itinerary\n--------------------------------------------------------\nStop\tDestination\t\tType\t\tActivity");
		singleLinkedList.printList();
		
		//Logic to sort through and remove all adventure types
		boolean hasAdventure = true;	
		
		while(hasAdventure) {
		Destination destination = singleLinkedList.removeDestination("adventure");
			if(destination != null) {
				doubleLinkedList.addDestination(destination);
				System.out.println("add");
			}
			else {
				hasAdventure = false;
			}
		}
		
		//Printing Rest of Values
		System.out.println("\nAdventures Removed Itinerary\n--------------------------------------------------------\nStop\tDestination\t\tType\t\tActivity");
		singleLinkedList.printList();
		
		System.out.println("\nAdventure Destinations in a double linked list printed backwards\n--------------------------------------------------------\nStop\tDestination\t\tType\t\tActivity");
		doubleLinkedList.printListBackwards();
		
		singleLinkedList.reverse();
		
		System.out.println("\nItinerary printed backwards\n--------------------------------------------------------\nStop\tDestination\t\tType\t\tActivity");
		singleLinkedList.printList();
		
		
	}
}

class Destination{
	private int stop;
	private String type;
	private String name;
	private String activity;
	
	public Destination(int stop, String type, String activity) {
		this.stop = stop;
		this.type = type;
		this.activity = activity;
	}
	
	public int getStop() {
		return stop;
	}
	
	public String getName() {
		return name;
	}
	
	public String getType() {
		return type;
	}
	
	public String getActivity() {
		return activity;
	}
	
	public void setStop(int stop) {
		this.stop = stop;
	}
	
	public void setType(String type) {
		this.type = type;
	}
	
	public void setName(String name) {
		this.name = name;
	}
	
	public void setActivity(String activity) {
		this.activity = activity;
	}
	
	
	@Override
	public String toString() {
		String returnString = stop + "\t" + name + "\t\t" + type + "\t\t" + activity;
		
		return returnString;
	}
}

class ItineraryLinkedList{
	private Node head;
	
	//takes in a destination object
	public void addByStopNumber(Destination destinationToAdd) {
		//turns it into a node
		Node aNode = new Node(destinationToAdd);
		
		//if there is no head yet OR the value is less than the head value
		if(head == null || destinationToAdd.getStop() < head.destination.getStop()) {
			
			//That is the new head value
			aNode.next = head;
			head = aNode;
		}
		//Otherwise
		else {
			//start at the head
			Node current = head;
			//while the linked list has values AND the next value is less than the value to add
			while(current.next != null && current.next.destination.getStop() < destinationToAdd.getStop()) {
				//keep iterating
				current = current.next;
			}
			//if none of these conditions are true, the value to add goes next
			aNode.next = current.next;
			current.next = aNode;
		}
	}
	
	public Destination removeDestination(String typeToRemove) {
		
		Destination removedDestination = null;
		
		Node current = head;
		Node prev = null;
		
		//if the head is one of the type to be removed
		if(head.destination.getType().equals(typeToRemove)) {
			//head is moved up one and returned
			removedDestination = head.destination;
			head = head.next;
		}
		//while there are values and the current value doesnt equal one of the removed values
		while (current != null && !current.destination.getType().equals(typeToRemove)) {
	       //move forward
			prev = current;
	        current = current.next;
	    }

	    // If the destination with the specified type is found, remove it
	    if (current != null) {
	        removedDestination = current.destination;
	        //if there are no more values before
	        if (prev != null) {
	            //move forward one
	        	prev.next = current.next;
	        }
	    }
		//returns the value
		return removedDestination;
	}
	
	public void reverse() {
		Node prev = null;
		Node current = head;
		Node next = null;
		//while there are values
		while(current != null) {
			//move backwards
			next = current.next;
			current.next = prev;
			prev = current;
			current = next;
		}
		//the head is the 
		head = prev;
	}
	
	//iterates and prints the values
	public void printList() {
		Node current = head;
		while(current != null) {
			System.out.println(current.destination);
			current = current.next;
		}
	}
	
	class Node{
		private Destination destination;
		private Node next;
		
		public Node(Destination destination) {
			this.destination = destination;
			this.next = null;
		}
		
	}
}

class DoubleLinkedList{
	private Node head;
	private Node tail;
	
	
	public void addDestination (Destination destination) {
		Node aNode = new Node(destination);
		//if there is no head(aka this is the first value)
		if(head == null) {
			//the list is only one long, head and tail are the same
			head = aNode;
			tail = aNode;
		}
		else {
			//other wise add the node to the end
			tail.next = aNode;
			aNode.previous = tail;
			tail = aNode;
		}
	}
	
	public void printListBackwards() {
		Node current = tail;
		//while there are values
		while(current != null) {
			//move backward and print
			System.out.println(current.destination.toString());
			current = current.previous;
		}
	}
	
	class Node{
		private Destination destination;
		private Node previous;
		private Node next;
		
		public Node(Destination destination) {
			this.destination = destination;
			this.next = null;
			this.previous = null;
		}
	}
}