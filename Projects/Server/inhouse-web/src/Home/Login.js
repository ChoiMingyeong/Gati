// Login.jsx
import React, { useState } from 'react';
import { SceneEnum, useGlobalStore } from '../store/store';
import './Login.css';

export default function Login() {
    const { setScene } = useGlobalStore();
    const [id, setId] = useState('');
    const [pw, setPw] = useState('');

    const handleLogin = (e) => {
        e.preventDefault();
        console.log("id:", id);
        console.log("pw:", pw);
        alert("접속완료");
        setScene(SceneEnum.Home);
    };

    return (
        <div id="login">
            <div id="container">
                <div id="top">
                    <h2></h2>
                </div>
                <h2>Login</h2>
                <form id="login-form" onSubmit={handleLogin}>
                    <div className="login_input">
                        <input
                            type="text"
                            placeholder="ID"
                            value={id}
                            onChange={(e) => setId(e.target.value)}
                        />
                        <input
                            type="password"
                            placeholder="PASSWORD"
                            value={pw}
                            onChange={(e) => setPw(e.target.value)}
                        />
                    </div>
                    <div className="submit_button">
                        <input type="submit" value="Login" />
                    </div>
                </form>
            </div>
        </div>
    );
}
