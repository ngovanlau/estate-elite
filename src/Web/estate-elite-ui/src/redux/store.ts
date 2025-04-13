import { combineReducers, configureStore } from "@reduxjs/toolkit";
import { createWrapper, HYDRATE } from "next-redux-wrapper";

// Import reducers

const rootReducer = (state, action) => {
	if (action.type === HYDRATE) {
		// Hydrate từ server state
		const nextState = {
			...state, // giữ nguyên state cũ
			...action.payload, // áp dụng state từ server
		};
		return nextState;
	}
	return combineReducers({
		// Add reducer
	})(state, action);
};

// Create store config function
const makeStore = () =>
	configureStore({
		reducer: rootReducer,
		devTools: process.env.NODE_ENV !== "production",
	});

// Export store wrapper
export const wrapper = createWrapper(makeStore);

// Export store type
export type AppStore = ReturnType<typeof makeStore>;
export type RootState = ReturnType<AppStore["getState"]>;
export type AppDispatch = AppStore["dispatch"];
