import { ACTION_TYPES } from "../actions/ProjectsAct";
const initialState ={
    list:[]
}

// poniżej Projects czy inna nazwa?
export const ProjectsRed = (state=initialState, action) => {

    switch (action.type) {
        case ACTION_TYPES.FETCH_ALL:
            return {
                ...state,
                list: [...action.payload]
            }

        default:
            return state
    }
}