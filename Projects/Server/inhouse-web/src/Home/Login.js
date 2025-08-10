// Login.jsx
import React, { useState } from "react";
import { SceneEnum, useGlobalStore } from "../store/store";
import "./Login.css";

export default function Login() {
  const { setScene } = useGlobalStore();
  const [email, setEmail] = useState("");
  const [pw, setPw] = useState("");

  const handleLogin = (e) => {
    e.preventDefault();
    console.log("email:", email);
    console.log("pw:", pw);
    alert("접속완료");
    setScene(SceneEnum.Home);
  };

  return (
    <div className="login">
      <form className="login__form" onSubmit={handleLogin}>
        <h1 className="login__title">Login</h1>

        <div className="login__inputs">
          <div className="login__box">
            <input
              type="email"
              placeholder="Email ID"
              value={email}
              onChange={(e) => setEmail(e.target.value)}
              required
              className="login__input" />
            <i className="ri-mail-fill"></i>
          </div>

          <div className="login__box">
            <input
              type="password"
              placeholder="Password"
              value={pw}
              onChange={(e) => setPw(e.target.value)}
              required
              className="login__input" />
            <i className="ri-lock-2-fill"></i>
          </div>
        </div>

        <div className="login__check">
          <div className="login__check-box">
            <input type="checkbox" className="login__check-input" id="user-check" />
            <label htmlFor="user-check" className="login__check-label">
              Remember me
            </label>
          </div>
        </div>

        <button type="submit" className="login__button">
          Login
        </button>

        <div className="login__copyright">
          Copyright 2025. kolyn. All rights reserved.
        </div>
      </form>
    </div>
  );
}
