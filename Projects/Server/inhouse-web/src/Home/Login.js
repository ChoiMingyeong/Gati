import React, { useEffect, useState } from "react";
import { useNavigate } from 'react-router-dom';
import { useGlobalStore } from "../store/store";
import "./Login.css";

// XSS 방지용 간단 Sanitizer
const sanitizeInput = (str) => {
  return str.replace(/[<>"'`;(){}]/g, ""); 
}

export default function Login() {
  const { PostApiAsync, validateToken, setToken } = useGlobalStore();
  const navigate = useNavigate();
  const [email, setEmail] = useState("");
  const [password, setPw] = useState("");

  useEffect(() => {
    validateToken((res) => {
      if (res.valid) {
        // 로그인 상태
        navigate('/home/account');
      }
    });
  },[]);

  const login = (cb) => {
    //console.log("email : ", email);
    //console.log("password : ", password);
    const safeEmail = sanitizeInput(email);
    const safePw = sanitizeInput(password);

    PostApiAsync("login", { email: safeEmail, password: safePw }, (data) => {
      if (data.success) {
        localStorage.setItem("token", data.token);
        setToken(data.token); // context state 반영
        navigate('/home/account');
      }
      cb(data);
    })
  }

  const handleLogin = (e) => {
    e.preventDefault();
    login((res) => {
      if (!res.success) {
        alert("로그인 실패");
      }
    })
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
              value={password}
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
