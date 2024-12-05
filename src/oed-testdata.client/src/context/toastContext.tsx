import React, { createContext, useState, useContext, useCallback } from "react";
import { ToastContainer } from "../components/toasts";
import { Severity, Toast, ToastContextType } from "../interfaces/IToast";

const ToastContext = createContext<ToastContextType>({
  toasts: [],
  addToast: () => {},
});

interface ToastProviderProps {
  children: React.ReactNode;
}

export const ToastProvider: React.FC<ToastProviderProps> = ({ children }) => {
  const [toasts, setToasts] = useState<Toast[]>([]);

  const addToast = useCallback((message: string, type: Severity) => {
    const id = Date.now();
    const newToast = {
      id,
      message,
      type,
    };

    setToasts((prevToasts) => [...prevToasts, newToast]);

    setTimeout(() => {
      setToasts((prevToasts) => prevToasts.filter((toast) => toast.id !== id));
    }, 5000);
  }, []);

  return (
    <ToastContext.Provider value={{ toasts, addToast }}>
      {children}
      <ToastContainer toasts={toasts} />
    </ToastContext.Provider>
  );
};

export const useToast = () => {
  const context = useContext(ToastContext);
  if (!context) {
    throw new Error("useToast must be used within a ToastProvider");
  }
  return context;
};
