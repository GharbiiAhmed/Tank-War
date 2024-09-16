import User from "../models/user.js";
import nodemailer from 'nodemailer';
export function signin(req, res) {
  User.findOne({ username: req.body.username, password: req.body.password })
    .then((doc) => {
      res.status(200).json(doc);
    })
    .catch((err) => {
      res.status(500).json({ error: err });
    });
}

const transporter = nodemailer.createTransport({
  service: 'gmail', // use your email service
  auth: {
    user: 'sahbi.skirilex@gmail.com', // your email address
    pass: 'otfa yyou bkit pfri'   // your email password
  }
});

export function signup(req, res) {
  User.create({
    username: req.body.username,
    password: req.body.password,
    adresse: req.body.adresse,
    victoires: "0",
    defaites: "0"
  })
    .then((newUser) => {
      // Send an email after successful signup
      const mailOptions = {
        from: '"Your Name" <sahbi.skirilex@gmail.com>',
        to: req.body.adresse, // user email address from signup form
        subject: 'Welcome to Our Platform!',
        text: `Hello ${newUser.username},\n\nThank you for signing up! We're excited to have you on board.\n\nBest Regards,\nYour Company Name`
      };

      transporter.sendMail(mailOptions, (error, info) => {
        if (error) {
          console.log('Error sending email:', error);
          return res.status(500).json({ error: 'Signup successful, but email sending failed.' });
        }
        console.log('Email sent:', info.response);
        res.status(200).json({
          username: newUser.username,
          password: newUser.password,
          adresse: newUser.adresse,
          victoires: newUser.victoires,
          defaites: newUser.defaites,
          message: 'Signup successful and email sent!'
        });
      });
    })
    .catch((err) => {
      res.status(500).json({ error: err });
    });
}

export function putOnce(req, res) {
  let newUser = {};
  if(req.file == undefined) {
    newUser = {
      username: req.body.username,
      password: req.body.password,
      adresse: req.body.adresse
    }
  }
  else {
    newUser = {
      username: req.body.username,
      password: req.body.password,
      adresse: req.body.adresse,
     
    }
  }
  User.findByIdAndUpdate(req.params.id, newUser)
    .then((doc1) => {
      User.findById(req.params.id)
        .then((doc2) => {
          res.status(200).json(doc2);
        })
        .catch((err) => {
          res.status(500).json({ error: err });
        });
    })
    .catch((err) => {
      res.status(500).json({ error: err });
    });
    
}
// Add +1 to victoires
export function victoiresadd(req, res) {
  User.findByIdAndUpdate(
    req.params.id,
    { $inc: { victoires: 1 } },
    { new: true }
  )
    .then((doc) => {
      res.status(200).json(doc);
    })
    .catch((err) => {
      res.status(500).json({ error: err });
    });
}

// Add +1 to defaites
export function defaitesadd(req, res) {
  User.findByIdAndUpdate(
    req.params.id,
    { $inc: { defaites: 1 } },
    { new: true }
  )
    .then((doc) => {
      res.status(200).json(doc);
    })
    .catch((err) => {
      res.status(500).json({ error: err });
    });
}

// Show victoires and defaites for specific user
export function history(req, res) {
  User.findById(req.params.id)
    .then((doc) => {
      res.status(200).json({
        victoires: doc.victoires,
        defaites: doc.defaites
      });
    })
    .catch((err) => {
      res.status(500).json({ error: err });
    });
}
export function updateGameResult(req, res) {
  const { result } = req.body;
  const userId = req.params.id;

  console.log(`Received game result for user ${userId}: ${result}`);

  if (result === 'win') {
    User.findByIdAndUpdate(
      userId,
      { $inc: { victoires: 1 } },
      { new: true }
    )
      .then((doc) => {
        console.log('Victory recorded successfully!');
        res.status(200).json({ message: 'Victory recorded successfully!', user: doc });
      })
      .catch((err) => {
        console.log('Error recording victory:', err);
        res.status(500).json({ error: err });
      });
  } else if (result === 'lose') {
    User.findByIdAndUpdate(
      userId,
      { $inc: { defaites: 1 } },
      { new: true }
    )
      .then((doc) => {
        console.log('Defeat recorded successfully!');
        res.status(200).json({ message: 'Defeat recorded successfully!', user: doc });
      })
      .catch((err) => {
        console.log('Error recording defeat:', err);
        res.status(500).json({ error: err });
      });
  } else {
    console.log('Invalid game result received.');
    res.status(400).json({ error: 'Invalid game result' });
  }
}

export function forgotPassword(req, res) {
  const userEmail = req.body.adresse;

  // Find the user by email
  User.findOne({ adresse: userEmail })
    .then((user) => {
      if (!user) {
        return res.status(404).json({ error: 'User not found' });
      }

      // Send an email with the user's password
      const mailOptions = {
        from: '"Your Name" <sahbi.skirilex@gmail.com>',
        to: user.adresse,
        subject: 'Your Password Recovery',
        text: `Hello ${user.username},\n\nYour password is: ${user.password}\n\nPlease keep it secure.\n\nBest Regards,\nYour Company Name`
      };

      transporter.sendMail(mailOptions, (error, info) => {
        if (error) {
          console.log('Error sending email:', error);
          return res.status(500).json({ error: 'Failed to send email' });
        }
        console.log('Email sent:', info.response);
        res.status(200).json({ message: 'Password has been sent to your email' });
      });
    })
    .catch((err) => {
      res.status(500).json({ error: err });
    });
}
export function getUserStats(req, res) {
  const userId = req.params.id;

  User.findById(userId)
      .then((user) => {
          if (!user) {
              return res.status(404).json({ error: 'User not found' });
          }
          res.status(200).json({
              victoires: user.victoires,
              defaites: user.defaites
          });
      })
      .catch((err) => {
          res.status(500).json({ error: err.message });
      });
}
