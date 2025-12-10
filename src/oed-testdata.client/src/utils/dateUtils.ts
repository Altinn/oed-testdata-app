export function addDays(date: Date, days: number): Date {
  const newDate = new Date(date); // Create a copy
  newDate.setDate(newDate.getDate() + days);
  return newDate;
}

export function dateOnlyString(date: Date): string {
  const year = date.getFullYear();
  const month = (date.getMonth() + 1).toString().padStart(2, '0'); // Month is 0-indexed, so add 1
  const day = date.getDate().toString().padStart(2, '0');

  const formattedDate = `${year}-${month}-${day}`; // e.g., "2025-12-10"  
  return formattedDate;
}
  