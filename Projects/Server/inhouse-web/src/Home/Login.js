import React, { useEffect, useState } from "react";
import { useNavigate } from 'react-router-dom';
import { useGlobalStore } from "../store/store";
import "./Login.css";

export default function Login() {
  const { PostApiAsync, GetApiAsync } = useGlobalStore();
  const navigate = useNavigate();
  const [email, setEmail] = useState("");
  const [pw, setPw] = useState("");

  useEffect(() => {
    validateToken((res) => {
      if (res.valid) {
        // 로그인 상태
        navigate('/home/account');
      }
    });
  },[]);

  const login = (cb) => {
    PostApiAsync("Auth/login", {username, password }, (data) => {
      if (data.success) {
        localStorage.setItem("token", data.token);
        setToken(data.token);
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

  const validateToken = (cb) => {
    const savedToken = localStorage.getItem("token");
    if (!savedToken) {
      cb({ valid: false });
      return;
    }
    GetApiAsync('Auth/validate?token=${savedToken}', {}, cb);
  }

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
