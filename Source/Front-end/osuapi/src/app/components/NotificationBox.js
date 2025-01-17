"use client"
import * as React from "react";

export default function NotificationBox({visible,  text}){

  if (!visible) {
    return <></>;
  }

  return <div className="notification">{text}</div>;
}