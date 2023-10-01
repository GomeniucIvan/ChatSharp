import { configureStore } from '@reduxjs/toolkit';
import sessionsReducer from './sessionsSlice';

const store = configureStore({
    reducer: {
        sessions: sessionsReducer,
    }
});

export default store;