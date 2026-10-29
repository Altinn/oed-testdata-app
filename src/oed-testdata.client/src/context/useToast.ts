import { createContext, useContext } from "react";
import { ToastContextType } from "../interfaces/IToast";

export const ToastContext = createContext<ToastContextType>({
  toasts: [],
  addToast: () => {},
});

export const useToast = () => {
  const context = useContext(ToastContext);
  if (!context) {
    throw new Error("useToast must be used within a ToastProvider");
  }
  return context;
};
