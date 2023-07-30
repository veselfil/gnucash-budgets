import './App.css';
import {OpenAPI} from "./gc-client";
import {GnuCashContextProvider} from "./context/GnuCashContext";
import 'bootstrap/dist/css/bootstrap.min.css';
import BudgetsSetupPage from "./pages/BudgetsSetupPage/BudgetsSetupPage";
import NavigationBar from "./components/NavigationBar/NavigationBar";
import { createBrowserRouter, RouterProvider } from 'react-router-dom';
import BudgetsBalancePage from "./pages/BudgetsBalancePage/BudgetsBalancePage";

const router = createBrowserRouter([
    { 
        path: "/setup",
        element: <BudgetsSetupPage />
    },
    {
        path: "/balance",
        element: <BudgetsBalancePage />
    }
])

function App() {
  return (
      <div>
          <NavigationBar />
          <RouterProvider router={router} />
      </div>
  );
}

// TODO: Fix this
OpenAPI.BASE = "http://localhost:5192"

export default App;
