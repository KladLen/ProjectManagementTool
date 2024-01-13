import axios from "axios";

const baseUrl = "https://localhost:44359/api/"

export default {

    Projects(url = baseUrl + 'Project/') {
        return {
            fetchAll: () => axios.get(url)
            //fetchById: id => axios.get(url + id),
            //create: newRecord => axios.post(url + 'create/', newRecord),
            //update: (id, updateRecord) => axios.put(url + id, updateRecord),
            //delete: id => axios.delete(url + id)
        }
    }
}