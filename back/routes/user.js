import express from "express";
import { body } from "express-validator";
import multer from "../middlewares/multer-config.js";
import { signin, signup, putOnce, victoiresadd, defaitesadd, history, forgotPassword, getUserStats, updateGameResult} from "../controllers/user.js";

const router = express.Router();

router
  .route("/signin")
  .post(
    body("username").isLength({ min: 5 }),
    body("password").isLength({ min: 5 }), // corrected to "password"
    signin
  );

router.route("/signup").post(signup);

router
  .route("/:id")
  .put(
    body("username").isLength({ min: 5 }),
    body("password").isLength({ min: 5 }), // corrected to "password"
    putOnce
  );

router.route("/:id/victoires").post(victoiresadd);

router.route("/:id/defaites").post(defaitesadd);

router.route("/:id/history").get(history);
router.route("/:id/stats").get(getUserStats);

// Forgot password route
router.route("/forgot-password").post(
  body("adresse").isEmail(), // Validate email input
  forgotPassword
);
router.post('/user/:id/game-result', updateGameResult);

export default router;
