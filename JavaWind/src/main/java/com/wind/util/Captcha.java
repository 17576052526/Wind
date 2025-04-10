package com.wind.util;

import java.awt.*;
import java.awt.image.BufferedImage;
import java.util.Random;

public class Captcha {

    private static final int WIDTH = 100;
    private static final int HEIGHT = 30;
    private static final String CHARACTERS = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";

    public BufferedImage generateCaptchaImage(String captchaText) {
        BufferedImage image = new BufferedImage(WIDTH, HEIGHT, BufferedImage.TYPE_INT_RGB);
        Graphics2D g2d = image.createGraphics();

        // 绘制背景
        g2d.setColor(Color.WHITE);
        g2d.fillRect(0, 0, WIDTH, HEIGHT);

        // 绘制验证码字符
        g2d.setFont(new Font("Arial", Font.BOLD, 20));
        for (int i = 0; i < captchaText.length(); i++) {
            g2d.setColor(new Color(new Random().nextInt(255), new Random().nextInt(255), new Random().nextInt(255)));
            g2d.drawString(String.valueOf(captchaText.charAt(i)), 10 + i * 15, 20);
        }

        // 绘制干扰线
        Random random = new Random();
        for (int i = 0; i < 5; i++) {
            g2d.setColor(new Color(random.nextInt(255), random.nextInt(255), random.nextInt(255)));
            int x1 = random.nextInt(WIDTH);
            int y1 = random.nextInt(HEIGHT);
            int x2 = random.nextInt(WIDTH);
            int y2 = random.nextInt(HEIGHT);
            g2d.drawLine(x1, y1, x2, y2);
        }

        g2d.dispose();
        return image;
    }

    public String generateRandomString(int length) {
        Random random = new Random();
        StringBuilder sb = new StringBuilder();
        for (int i = 0; i < length; i++) {
            sb.append(CHARACTERS.charAt(random.nextInt(CHARACTERS.length())));
        }
        return sb.toString();
    }
}
