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

export function randomDateISO(start: Date, end: Date) {
  const date = new Date(start.getTime() + Math.random() * (end.getTime() - start.getTime()));
  return date.toISOString().split("T")[0];
}

export function dateFromCSharpTicks(ticks: string | number) {
    const TICKS_AT_UNIX_EPOCH = 621355968000000000n; // .NET ticks at 1970-01-01
    const ticksBigInt = BigInt(ticks);

    // Convert ticks → milliseconds
    const msSinceUnixEpoch = (ticksBigInt - TICKS_AT_UNIX_EPOCH) / 10000n;

    return new Date(Number(msSinceUnixEpoch));
}