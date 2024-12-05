export type Severity = "info" | "warning" | "success" | "danger";

export interface Toast {
  id: number;
  message: string;
  type: Severity;
}

export interface ToastContextType {
  toasts: Toast[];
  addToast: (message: string, type: Severity) => void;
}
