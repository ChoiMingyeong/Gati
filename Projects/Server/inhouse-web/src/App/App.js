// App.jsx
import React from 'react';
import { SceneEnum, useGlobalStore } from '../store/store';
import Login from '../Home/Login';
import Home from '../Home/Home';

export default function App() {
    const { scene } = useGlobalStore();

    return (
        <>
            {scene === SceneEnum.Login && <Login />}
            {scene === SceneEnum.Home && <Home />}
            {scene !== SceneEnum.Login && scene !== SceneEnum.Home && <div>Error</div>}
        </>
    );
}
