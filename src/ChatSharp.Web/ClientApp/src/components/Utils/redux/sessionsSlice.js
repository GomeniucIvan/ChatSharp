import { createSlice } from '@reduxjs/toolkit';

const sessionsSlice = createSlice({
    name: 'sessions',
    initialState: [],
    reducers: {
        setSessions: (state, action) => action.payload,
        addSession: (state, action) => {
            state.push(action.payload);
        }
    }
});

export const { setSessions, addSession } = sessionsSlice.actions;
export default sessionsSlice.reducer;