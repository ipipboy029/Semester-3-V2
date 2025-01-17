import { useState } from "react";

export default function useNotification() {
  const [visible, setVisible] = useState(false);
  const [text, setText] = useState("");

  const showNotification = () => {
    setVisible(true);
    setText(text);
    setTimeout(() => {
      setVisible(false);
    }, ms);
  };

  return {
    visible,
    text,
    showNotification
  };
}