import { Alert, Paragraph } from "@digdir/designsystemet-react";
import React from "react";
import "./style.css";
import { Toast } from "../../interfaces/IToast";

export const ToastContainer: React.FC<{ toasts: Toast[] }> = ({ toasts }) => {
  return (
    <aside className={`toast__container ${toasts.length ? "toast__container--visible" : ""}`}>
      {toasts.map((toast) => (
        <Alert
          key={toast.id}
          data-color={toast.type}
        >
          <Paragraph>{toast.message}</Paragraph>
        </Alert>
      ))}
    </aside>
  );
};
